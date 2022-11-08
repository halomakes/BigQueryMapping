using System.ComponentModel.DataAnnotations.Schema;
using BigQueryMapping;

namespace UsageTests;

public class Tests
{
    [Fact]
    public void ShouldHaveMethod()
    {
        var method = typeof(MeterReadingEntry).GetMethod("FromBigQueryRow");
        Assert.NotNull(method);
        Assert.Equal(typeof(MeterReadingEntry), method!.ReturnType);
        Assert.True(method!.IsStatic);
    }
    
    [Fact]
    public void ShouldImplementInterface()
    {
        var @interfaces = typeof(MeterReadingEntry).GetInterfaces();
        var matchingInterface =
            @interfaces.FirstOrDefault(i => i.FullName.StartsWith("BigQueryMapping.IBigQueryGenerated"));
        Assert.NotNull(@matchingInterface);
    }
}

[BigQueryMapped]
public partial class MeterReadingEntry
{
    [Column("PropertyID")] public string? PropertyId { get; set; }

    [Column("Property_RentRoll_ID")] public string? RentRollId { get; set; }

    [Column("header_serial_number")] public string? SerialNumber { get; set; }

    [Column("Gallons")] public double? Gallons { get; set; }

    [Column("Events")] public double? Events { get; set; }

    [Column("Flowtime")] public double? FlowTime { get; set; }

    [Column("hour")] public DateTime? Hour { get; set; }

    [Column("Running_hours")] public long? RunningHours { get; set; }

    [Column("CatchupFlag")] public string? CatchupFlag { get; set; }

    [Column("Leak_Status")] public string? LeakStatus { get; set; }

    [Column("Leak_Details")] public string? LeakDetails { get; set; }

    [Column("toilet_leak")] public double? ToiletLeak { get; set; }

    [Column("Miscellaneous_Leaks")] public double? MiscellaneousLeak { get; set; }

    [Column("daily_leak_gallon_total")] public double? DailyGallonsLeaked { get; set; }

    [Column("daily_leak_status")] public string? DailyLeakStatus { get; set; }

    [Column("Leak_Gallons_22_hours")] public double? LeakGallons22Hours { get; set; }

    [Column("DateLeakStarted")] public DateTime? DateLeakStarted { get; set; }

    [Column("Total_gallons_since_leak")] public double? GallonsSinceLastLeak { get; set; }

    [Column("last_leaking_at")] public DateTime? LastLeakingAt { get; set; }

    [Column("Developer")] public string? DeveloperName { get; set; }

    [Column("GPD_Filter")] public string? GpdFilter { get; set; }

    [Column("FlowtimeHHmm")] public string? FlowTimeFormatted { get; set; }

    [Column("ran_more_than_22h")] public string? RanMoreThan22Hours { get; set; }

    [Column("hourly_reading_status_num")] public long? HourlyReadingStatus { get; set; }

    [Column("Days_Repeating")] public long? DaysRepeating { get; set; }

    [Column("Unit_Details")] public string? UnitDetails { get; set; }

    [Column("Hours_running_55min_Daily")] public long? HoursRunningMoreThan55Minutes { get; set; }

    [Column("Leak_Gallons")] public double? LeakGallons { get; set; }

    [Column("Orange_Leak_Gallons")] public double? WarningLeakGallons { get; set; }

    [Column("Red_Leak_Gallons")] public double? UrgentLeakGallons { get; set; }

    [Column("Latest_Leak_Status")] public string? LatestLeakStatus { get; set; }

    [Column("duration_since_last_reading")]
    public string? TimeSinceLastReading { get; set; }

    [Column("DPOE_hourly_events_hot_cold")]
    public string? DpoeHourlyEvents { get; set; }

    [Column("DPOE_hourly_gallons_hot_cold")]
    public string? DpoeHourlyGallons { get; set; }

    [Column("DPOE_hourly_flowtime_hot_cold")]
    public string? DpoeHourlyFlowTime { get; set; }
}

