namespace Hangfire.JobManagement.Configuration;

public class JobManagementConfiguration
{
    public const string Position = "JobManagement";

    public string ConnectionString { get; set; }

    public DebugConfiguration Debug { get; set; } = new DebugConfiguration();
}

public class DebugConfiguration
{
    public bool Migrations { get; set; }
}
