using Hangfire.Console;
using Hangfire.JobManagement.Jobs.Attributes;
using Hangfire.JobManagement.Test.Jobs.Abstractions;
using Hangfire.JobManagement.Test.Jobs.Abstractions.Parameters;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace Hangfire.JobManagement.Test.Jobs.Recurring;

[JobManager("Current Recurring Job")]
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
    //[JobMethod]
    [DisplayName("Custom Recurring Job")]
    public override async Task ExecuteAsync(PerformContext context, JobParametersBase parameters, string title, CancellationToken cancellationToken)
    {
        // cancellation token
        cancellationToken = GetCancellationToken(context, cancellationToken);

        // progress bar
        var progressBar = context.WriteProgressBar();

        // parameters
        var jobParams = parameters as CustomRecurringJobParameters ?? new CustomRecurringJobParameters();

        try
        {
            context.WriteLine("");
            context.WriteLine("");
            context.WriteLine($"##################################################");
            context.WriteLine($"# Custom Recurring Job: (Parameter Value: {jobParams.Parameter})");
            context.WriteLine($"##################################################");
            context.WriteLine("");
            context.WriteLine("");

            // simulate work
            var totalDuration = TimeSpan.FromMinutes(3);
            var interval = TimeSpan.FromSeconds(10);

            int totalSteps = (int) (totalDuration / interval);
            int step = 0;

            while (step <= totalSteps)
            {
                int progress = (int) Math.Round((double) step / totalSteps * 100);

                if (step == totalSteps) break;

                await Task.Delay(interval);

                step++;

                progressBar.SetValue(progress);
                context.WriteLine($"Progress: {progress}%");
            }
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
            progressBar.SetValue(100);
        }
    }
}
