namespace Hangfire.JobManagement.Test.SampleServer.Configuration;

public class HangfireConfiguration
{
    public const string Position = "Hangfire";

    public string? ConnectionString { get; set; }

    public string? DashboardUrl { get; set; } 
}
