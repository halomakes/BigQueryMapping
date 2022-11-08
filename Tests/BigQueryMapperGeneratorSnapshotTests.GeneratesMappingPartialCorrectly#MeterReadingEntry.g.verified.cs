﻿//HintName: MeterReadingEntry.g.cs
// <auto-generated/>
namespace Test
{
    public partial class MeterReadingEntry 
    {
        public static MeterReadingEntry FromBigQueryRow(Google.Cloud.BigQuery.V2BigQueryRow row)
        {
            return new MeterReadingEntry
            {
                PropertyId = row["PropertyId"],
                RentRollId = row["RentRollId"],
                SerialNumber = row["SerialNumber"],
                Gallons = row["Gallons"],
                Events = row["Events"],
                FlowTime = row["FlowTime"],
                Hour = row["Hour"],
                RunningHours = row["RunningHours"],
                CatchupFlag = row["CatchupFlag"],
                LeakStatus = row["LeakStatus"],
                LeakDetails = row["LeakDetails"],
                ToiletLeak = row["ToiletLeak"],
                MiscellaneousLeak = row["MiscellaneousLeak"],
                DailyGallonsLeaked = row["DailyGallonsLeaked"],
                DailyLeakStatus = row["DailyLeakStatus"],
                LeakGallons22Hours = row["LeakGallons22Hours"],
                DateLeakStarted = row["DateLeakStarted"],
                GallonsSinceLastLeak = row["GallonsSinceLastLeak"],
                LastLeakingAt = row["LastLeakingAt"],
                DeveloperName = row["DeveloperName"],
                GpdFilter = row["GpdFilter"],
                FlowTimeFormatted = row["FlowTimeFormatted"],
                RanMoreThan22Hours = row["RanMoreThan22Hours"],
                HourlyReadingStatus = row["HourlyReadingStatus"],
                DaysRepeating = row["DaysRepeating"],
                UnitDetails = row["UnitDetails"],
                HoursRunningMoreThan55Minutes = row["HoursRunningMoreThan55Minutes"],
                LeakGallons = row["LeakGallons"],
                WarningLeakGallons = row["WarningLeakGallons"],
                UrgentLeakGallons = row["UrgentLeakGallons"],
                LatestLeakStatus = row["LatestLeakStatus"],
                TimeSinceLastReading = row["TimeSinceLastReading"],
                DpoeHourlyEvents = row["DpoeHourlyEvents"],
                DpoeHourlyGallons = row["DpoeHourlyGallons"],
                DpoeHourlyFlowTime = row["DpoeHourlyFlowTime"],
            }
        }
    }
}