using Hangfire.JobManagement.Test.SampleServer.Jobs;
using Microsoft.AspNetCore.Mvc;

namespace Hangfire.JobManagement.Test.SampleServer.Controllers;

[ApiController]
[Route("[controller]")]
public class JobsController : ControllerBase
{
    // logging
    private readonly ILogger<JobsController> _logger;

    // timezones
    public TimeZoneInfo TimeZoneInfoEst = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
    public TimeZoneInfo TimeZoneInfoCst = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

    public JobsController(ILogger<JobsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    ///  Kills all running jobs in Hangfire
    /// </summary>
    /// <returns></returns>
    [HttpPost("Kill-All")]
    public Task JobsKillAll()
    {
        // get all jobs
        var storage = JobStorage.Current;

        // Get the connection to the storage
        using (var connection = storage.GetConnection())
        {
            // Get the monitoring API
            var monitoringApi = storage.GetMonitoringApi();

            // processing jobs (we need to do this first... b/c of the job orchestrator job...)
            var processingJobs = monitoringApi.ProcessingJobs(0, int.MaxValue);

            foreach (var job in processingJobs)
            {
                try
                {
                    // Delete the job
                    BackgroundJob.Delete(job.Key);
                    _logger.LogInformation($"Job {job.Key}, has been killed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to delete job {job.Key}: {ex.Message}");
                }
            }

            var enqueuedJobsDefault = monitoringApi.EnqueuedJobs("default", 0, int.MaxValue);

            foreach (var job in enqueuedJobsDefault)
            {
                try
                {
                    // Delete the job
                    BackgroundJob.Delete(job.Key);
                    _logger.LogInformation($"Job: {job.Key}, Enqueued Default has been killed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to delete job {job.Key}: {ex.Message}");
                }
            }

            // processing jobs
            var processingJobs2 = monitoringApi.ProcessingJobs(0, int.MaxValue);

            foreach (var job in processingJobs2)
            {
                try
                {
                    // Delete the job
                    BackgroundJob.Delete(job.Key);
                    _logger.LogInformation($"Job: {job.Key}, Processing Job has been killed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to delete job {job.Key}: {ex.Message}");
                }
            }

            // failed jobs
            var failedJobs = monitoringApi.FailedJobs(0, int.MaxValue);

            foreach (var job in failedJobs)
            {
                try
                {
                    // Delete the job
                    BackgroundJob.Delete(job.Key);
                    _logger.LogInformation($"Job: {job.Key}, Failed Job has been killed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to delete job {job.Key}: {ex.Message}");
                }
            }

            // succeeded jobs
            var succeededJobs = monitoringApi.SucceededJobs(0, int.MaxValue);

            foreach (var job in succeededJobs)
            {
                try
                {
                    // Delete the job
                    BackgroundJob.Delete(job.Key);
                    _logger.LogInformation($"Job: {job.Key}, Succeeded Job has been killed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Failed to delete job {job.Key}: {ex.Message}");
                }
            }
        }

        return Task.CompletedTask;
    }

    /// <summary>
    ///  Kills a job by the Job ID in Hangfire
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    [HttpPost("Kill")]
    public Task JobsKillById(string jobId)
    {
        BackgroundJob.Delete(jobId);
        return Task.CompletedTask;
    }

    [HttpPost("Setup")]
    public Task CreateJob()
    {
        // cst (default)
        var recurringJobOptionsCst = new RecurringJobOptions
        {
            TimeZone = TimeZoneInfoCst
        };

        // est 
        var recurringJobOptionsEst = new RecurringJobOptions
        {
            TimeZone = TimeZoneInfoEst

        };

        // jobs
        RecurringJob.AddOrUpdate<CustomRecurringJob>(CustomRecurringJob.JobName, job => job.ExecuteAsync(null, null, "Custom Job Title", CancellationToken.None), Cron.Daily(3, 0), recurringJobOptionsEst);

        //RecurringJob.AddOrUpdate<CustomRecurringJob>(CustomRecurringJob.JobName, job => job.ExecuteAsync(null, null, CancellationToken.None), Cron.Daily(3, 0), recurringJobOptionsEst);

        return Task.CompletedTask;
    }

    [HttpPost("Remove")]
    public Task RemoveJobs()
    {
        // remove if exists...
        RecurringJob.RemoveIfExists(CustomRecurringJob.JobName);

        return Task.CompletedTask;
    }
}
