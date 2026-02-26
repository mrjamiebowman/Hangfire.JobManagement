namespace Hangfire.JobManagement.Test.SampleServer.Jobs.Parameters;

public class CustomRecurringJobParameters : JobParametersBase
{
    public string? Parameter { get; set; }
}
