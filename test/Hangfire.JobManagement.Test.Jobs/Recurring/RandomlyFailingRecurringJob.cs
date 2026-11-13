using Hangfire.Console;
using Hangfire.JobManagement.Jobs.Attributes;
using Hangfire.JobManagement.Test.Jobs.Abstractions;
using Hangfire.JobManagement.Test.Jobs.Abstractions.Parameters;
using Hangfire.Server;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Diagnostics;

namespace Hangfire.JobManagement.Test.Jobs.Recurring;

[JobManager("Randomly.Failling.Job")]
public class RandomlyFailingRecurringJob : JobBase
{
    // logging
    private ILogger<RandomlyFailingRecurringJob> _logger;

    // vars
    public static string JobName { get; } = "Randomly.Failling.Job";

    public RandomlyFailingRecurringJob(ILogger<RandomlyFailingRecurringJob> logger)
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
    [DisplayName("Randomly Failling Job")]
    public override async Task ExecuteAsync(PerformContext context, JobParametersBase parameters, string title, CancellationToken cancellationToken)
    {
        using var activity = OTel.Application.StartActivity($"{nameof(CustomRecurringJob)}.{nameof(ExecuteAsync)}", ActivityKind.Internal);
        activity?.SetTag("job.name", JobName);

        var tags = new TagList
        {
            { "job.name", JobName }
        };

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
            context.WriteLine($"# Randomly Failing Job: (Parameter Value: {jobParams.Parameter})");
            context.WriteLine($"##################################################");
            context.WriteLine("");
            context.WriteLine("");

            // simulate work
            var totalDuration = TimeSpan.FromSeconds(10);
            await Task.Delay(totalDuration);

            // randomly fail
            if (Random.Shared.Next(2) == 0)
            {
                throw new InvalidOperationException("Fake 50/50 failure for OpenTelemetry testing.");
            }

            activity?.SetStatus(ActivityStatusCode.Ok);

            tags.Add(Spans.Status, Spans.Values.Success);
            OTel.Meters.AddJobRun(tags);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.AddException(ex);

            _logger.LogError(ex, "{jobName}: {error}", JobName, ex.Message);
            context.WriteLine($"ERROR: {ex.Message}");

            tags.Add(Spans.Status, Spans.Values.Failure);
            OTel.Meters.AddJobRun(tags);

            throw;
        }
        finally
        {
            // finish progress bar
            progressBar.SetValue(100);
        }
    }
}
