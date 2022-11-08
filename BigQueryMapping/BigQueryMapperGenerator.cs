using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace BigQueryMapping;

[Generator]
public class BigQueryMapperGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // add marker attribute
        context.RegisterPostInitializationOutput(ctx =>
            ctx.AddSource("BigQueryMappedAttribute.g.cs", SourceText.From(Attribute, Encoding.UTF8)));

        // get classes
        IncrementalValuesProvider<ClassDeclarationSyntax> classDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => s is ClassDeclarationSyntax c && c.AttributeLists.Any(),
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx)
            )
            .Where(static m => m is not null)!;

        IncrementalValueProvider<(Compilation Compilation, ImmutableArray<ClassDeclarationSyntax>Syntaxes)>
            compilationAndClasses = context.CompilationProvider.Combine(classDeclarations.Collect());

        context.RegisterSourceOutput(compilationAndClasses,
            static (spc, source) => Execute(source.Compilation, source.Syntaxes, spc));

        static ClassDeclarationSyntax? GetSemanticTargetForGeneration(GeneratorSyntaxContext context)
        {
            var classDeclarationSyntax = (ClassDeclarationSyntax)context.Node;

            foreach (var attributeListSyntax in classDeclarationSyntax.AttributeLists)
            {
                foreach (var attributeSyntax in attributeListSyntax.Attributes)
                {
                    var fullName = context.SemanticModel.GetTypeInfo(attributeSyntax).Type?.ToDisplayString();

                    if (fullName == "BigQueryMapping.BigQueryMappedAttribute")
                        return classDeclarationSyntax;
                }
            }

            return null;
        }

        static void Execute(Compilation compilation, ImmutableArray<ClassDeclarationSyntax> classes,
            SourceProductionContext context)
        {
            try
            {
                if (classes.IsDefaultOrEmpty)
                    return;

                var distinctClasses = classes.Distinct();

                var classesToGenerate = GetTypesToGenerate(compilation, distinctClasses, context.CancellationToken);

                foreach (var classToGenerate in classesToGenerate)
                {
                    var result = GeneratePartialClass(classToGenerate);
                    context.AddSource($"{classToGenerate.RowClass.Name}.g.cs", SourceText.From(result, Encoding.UTF8));
                }
            }
            catch (Exception e)
            {
                var descriptor = new DiagnosticDescriptor(id: "BQD001",
                    title: "Error creating bigquery mapper",
                    messageFormat: "{0} {1}",
                    category: "BigQueryMapperGenerator",
                    DiagnosticSeverity.Error,
                    isEnabledByDefault: true);

                context.ReportDiagnostic(Diagnostic.Create(descriptor, null, e.Message, e.StackTrace));
            }
        }
    }

    static IEnumerable<ClassToGenerate> GetTypesToGenerate(Compilation compilation,
        IEnumerable<ClassDeclarationSyntax> classes,
        CancellationToken ct)
    {
        foreach (var @class in classes)
        {
            Debug.WriteLine($"Checking class {@class}");
            ct.ThrowIfCancellationRequested();
            var semanticModel = compilation.GetSemanticModel(@class.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(@class) is not INamedTypeSymbol classSymbol)
                continue;

            var info = new ClassToGenerate(classSymbol, new());
            foreach (var member in classSymbol.GetMembers())
            {
                if (member is IPropertySymbol propertySymbol)
                {
                    if (propertySymbol.DeclaredAccessibility == Accessibility.Public)
                    {
                        if (propertySymbol.SetMethod is not null)
                        {
                            var columnName = propertySymbol.Name;
                            var columnAttribute = propertySymbol.GetAttributes().FirstOrDefault(a =>
                                a.AttributeClass!.ToDisplayString() == typeof(ColumnAttribute).FullName);
                            if (columnAttribute is not null)
                            {
                                if (!columnAttribute.NamedArguments.IsDefaultOrEmpty)
                                {
                                    var nameArg = columnAttribute.NamedArguments
                                        .FirstOrDefault(n => n.Key == nameof(ColumnAttribute.Name));
                                    if (nameArg.Value.Value is string name)
                                    {
                                        columnName = name;
                                    }
                                }
                            }

                            info.Properties.Add((columnName, propertySymbol));
                        }
                    }
                }
            }

            yield return info;
        }
    }


    static string GeneratePartialClass(ClassToGenerate c)
    {
        var sb = new StringBuilder();
        sb.Append($@"// <auto-generated/>
namespace {c.RowClass.ContainingNamespace.Name}
{{
    public partial class {c.RowClass.Name} 
    {{
        public static {c.RowClass.Name} FromBigQueryRow(Google.Cloud.BigQuery.V2.BigQueryRow row)
        {{
            return new {c.RowClass.Name}
            {{");

        foreach (var (columnName, property) in c.Properties)
        {
            // would like to check if key exists but don't see any sort of ContainsKey implemented on BigQueryRow
            var tempName = $"___{property.Name}";
            var basePropertyType = property.Type.WithNullableAnnotation(NullableAnnotation.None).ToDisplayString();
            if (basePropertyType.EndsWith("?"))
            {
                basePropertyType = basePropertyType.Substring(default, basePropertyType.Length - 1);
            }

            sb.Append($@"
                {property.Name} = row[""{columnName}""] is {basePropertyType} {tempName} ? {tempName} : default,");
        }

        sb.Append($@"
            }};
        }}
    }}
}}");
        return sb.ToString();
    }

    private record struct ClassToGenerate(INamedTypeSymbol RowClass,
        List<(string ColumnName, IPropertySymbol Property)> Properties);

    public const string Attribute = /* lang=csharp */ @"// <auto-generated/>
namespace BigQueryMapping{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class BigQueryMappedAttribute : System.Attribute
    {
    }
}";
}