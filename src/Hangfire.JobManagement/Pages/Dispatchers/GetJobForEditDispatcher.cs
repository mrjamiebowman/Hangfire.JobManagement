using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.JobManagement.Models;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers;

internal sealed class GetJobForEdit : IDashboardDispatcher
{
    private readonly IStorageConnection _connection;

    public GetJobForEdit() {
        _connection = JobStorage.Current.GetConnection();
    }

    public async Task Dispatch([NotNull] DashboardContext dashboardContext) 
    {
        using var activity = OTel.Application.StartActivity($"{nameof(GetJobForEdit)}.{nameof(Dispatch)}");

        var response = new Response() { Status = true };

        if (!"GET".Equals(dashboardContext.Request.Method, StringComparison.InvariantCultureIgnoreCase)) {
            dashboardContext.Response.StatusCode = 405;

            return;
        }

        var jobId = dashboardContext.Request.GetQuery("Id");
        var recurringJob = _connection.GetRecurringJobs().FirstOrDefault(x => x.Id == jobId);

        if (recurringJob == null) {
            response.Status = false;
            response.Message = "Job not found";

            await dashboardContext.Response.WriteAsync(JsonConvert.SerializeObject(response));

            return;
        }

        // validate: method
        // TODO: this happens when the jobs can't be found.

        // validate: class
        // TODO: this happens when the jobs can't be found.

        var periodicJob = new PeriodicJob {
            Id = recurringJob.Id,
            Cron = recurringJob.Cron,
            CreatedAt = recurringJob.CreatedAt,
            Error = recurringJob.Error,
            //                LastExecution = recurringJob.LastExecution,
            Method = recurringJob?.Job?.Method?.Name,
            Class = recurringJob?.Job?.Method?.ReflectedType?.FullName,
            Queue = recurringJob.Queue,
            LastJobId = recurringJob.LastJobId,
            LastJobState = recurringJob.LastJobState,
            //                NextExecution = recurringJob.NextExecution,
            Removed = recurringJob.Removed,
            TimeZoneId = recurringJob.TimeZoneId
        };

        response.Object = periodicJob;

        await dashboardContext.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}

