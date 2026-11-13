namespace Hangfire.JobManagement.Test.Jobs.Configuration;

public class HangfireConfiguration
{
    public const string Position = "Hangfire";

    public string? ConnectionString { get; set; }

    public string? DashboardUrl { get; set; } 
}
