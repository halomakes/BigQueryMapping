# BigQueryMapper
Source generator utility for converting [BigQueryRow](https://cloud.google.com/dotnet/docs/reference/Google.Cloud.BigQuery.V2/latest/Google.Cloud.BigQuery.V2.BigQueryRow)s to your DTO. 

## Usage

**Install NuGet packages**
```bash
dotnet add package BigQueryMapping
dotnet add package BigQueryMapping.Generators
```

**Apply attribute to your partial class**

Any property with an accessible setter will be included in mapping.  You can specify a column name using a [ColumnAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.schema.columnattribute).

```csharp
using BigQueryMapping

[BigQueryMapped]
public partial class MyRowDto
{
  public string SomeId { get; set; }
  
  [Column("some_RiDiCuLoUs_name")]
  public string SensibleName { get; set; }
}
```

The souce generator will implement `IBigQueryGenerated<MyRowDto>` which has a static method `FromBigQueryRow(BigQueryRow row)` that you can use to get your DTO out.

```csharp
var rows = await _bigQuery.ExecuteQueryAsync(sql, parameters, cancellationToken, new());

foreach (var row in rows)
{
    yield return MyRowDto.From(row);
}
```
