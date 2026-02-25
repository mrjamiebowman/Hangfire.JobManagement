namespace Hangfire.JobManagement.Test.SampleServer.Jobs.Parameters;

public class CustomRecurringJobParameters : JobParametersBase
{
    public int? Parameter { get; set; }
}
