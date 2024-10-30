using Hangfire.Annotations;
using Hangfire.JobManagement.Core;
using Hangfire.JobManagement.Models;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers;

internal sealed class GetJobDispatcher : Dashboard.IDashboardDispatcher
{
    private readonly IStorageConnection _connection;

    public GetJobDispatcher() {
        _connection = JobStorage.Current.GetConnection();
    }

    public async Task Dispatch([NotNull] Dashboard.DashboardContext context) {
        using var activity = OTel.Application.StartActivity($"{nameof(GetJobDispatcher)}.{nameof(Dispatch)}");

        if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase)) {
            context.Response.StatusCode = 405;
            return;
        }

        var recurringJob = _connection.GetRecurringJobs();
        var periodicJob = new List<PeriodicJob>();

        CultureInfo US_TimeFormat = new CultureInfo("en-US");

        if (recurringJob.Count > 0) {
            recurringJob.ForEach((x) => {
                periodicJob.Add(new PeriodicJob {
                    Id = x.Id,
                    Cron = x.Cron,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.Value.ChangeTimeZone(x.TimeZoneId) : new DateTime(),
                    Error = x.Error,
                    LastExecution = x.LastExecution.HasValue ? x.LastExecution.Value.ChangeTimeZone(x.TimeZoneId).ToString("G", US_TimeFormat) : "N/A",
                    Method = x.Job?.Method.Name,
                    JobState = "Running",
                    Class = x.Job?.Type.Name,
                    Queue = x.Queue,
                    LastJobId = x.LastJobId,
                    LastJobState = x.LastJobState,
                    NextExecution = x.NextExecution.HasValue ? x.NextExecution.Value.ChangeTimeZone(x.TimeZoneId).ToString("G", US_TimeFormat) : "N/A",
                    Removed = x.Removed,
                    TimeZoneId = x.TimeZoneId
                });
            });
        }

        // stopped jobs
        var stoppedJobs = JobAgent.GetAllJobStopped();

        // cross reference (this can happen when adding/deleting jobs while they are paused.)
        var matchedRunningJobs = stoppedJobs.Where(item1 => periodicJob.Any(item2 => item2.Id == item1.Id)).ToList();

        foreach (var job in matchedRunningJobs) {
            JobAgent.StartBackgroundJob(job.Id);
            stoppedJobs.Remove(job);
        }

        // add job was stopped:
        periodicJob.AddRange(stoppedJobs);

        // sort
        periodicJob = periodicJob.OrderBy(x => x.Id).ToList();

        await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
    }
}
