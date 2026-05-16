namespace Hangfire.JobManagement.Configuration;

public class JobManagementConfiguration
{
    public const string Position = "JobManagement";

    public string? ConnectionString { get; set; }

    /// <summary>
    ///  Default Time Zone
    ///  i.e., "Eastern Standard Time", "Central Standard Time"
    /// </summary>
    public string? DefaultTimeZone { get; set; }
}
