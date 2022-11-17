namespace BigQueryMapping
{
    public interface IBigQueryGenerated<TRow>
    {
        static abstract TRow FromBigQueryRow(Google.Cloud.BigQuery.V2.BigQueryRow row);
    }
}