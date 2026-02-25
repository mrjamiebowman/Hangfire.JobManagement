using Hangfire.Console;
using Hangfire.JobManagement.Test.SampleServer.Jobs.Parameters;
using Hangfire.Server;
using System.ComponentModel;

namespace Hangfire.JobManagement.Test.SampleServer.Jobs;

public class CustomRecurringJob : JobBase
{
    // logging
    private ILogger<CustomRecurringJob> _logger;

    // vars
    public static string JobName { get; } = "Custom.Recurring.Job";

    public CustomRecurringJob(ILogger<CustomRecurringJob> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///  Custom Recurring Job
    /// </summary>
    /// <param name="context"></param>
    /// <param name="parameters"></param>
    /// <param name="title"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [DisplayName("Custom Recurring Job")]
    public override async Task ExecuteAsync(PerformContext context, JobParametersBase parameters, string title, CancellationToken cancellationToken)
    {
        // cancellation token
        cancellationToken = GetCancellationToken(context, cancellationToken);

        // progress bar
        var progress = context.WriteProgressBar();

        // parameters
        var jobParams = parameters as CustomRecurringJobParameters ?? new CustomRecurringJobParameters();

        try
        {
            context.WriteLine("");
            context.WriteLine("");
            context.WriteLine($"###############################################");
            context.WriteLine($"# Custom Recurring Job: (Parameter Value: {jobParams.Parameter})");
            context.WriteLine($"###############################################");
            context.WriteLine("");
            context.WriteLine("");

            // simulate work
            await Task.Delay(TimeSpan.FromMinutes(3).Milliseconds);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{jobName}: {error}", JobName, ex.Message);
            context.WriteLine($"ERROR: {ex.Message}");
            throw;
        }
        finally
        {
            // finish progress bar
            progress.SetValue(100);
        }
    }
}
