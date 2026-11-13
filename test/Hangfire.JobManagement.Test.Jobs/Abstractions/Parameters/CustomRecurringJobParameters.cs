namespace Hangfire.JobManagement.Test.Jobs.Abstractions.Parameters;

public class CustomRecurringJobParameters : JobParametersBase
{
    public string? Parameter { get; set; }
}
