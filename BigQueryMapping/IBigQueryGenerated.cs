﻿namespace BigQueryMapping
{
    public interface IBigQueryGenerated<TRow>
    {
        static TRow FromBigQueryRow(Google.Cloud.BigQuery.V2.BigQueryRow row) =>
            throw new System.NotImplementedException();
    }
}