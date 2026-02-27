using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.JobManagement.Models;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Dashboard.Pages.Dispatchers;

internal sealed class GetJobForEdit : IDashboardDispatcher
{
    // logging
    private readonly ILogger<GetJobForEdit> _logger;

    // hangfire
    private readonly IStorageConnection _connection;

    public GetJobForEdit(ILogger<GetJobForEdit> logger)
    {
        _logger = logger;
        _connection = JobStorage.Current.GetConnection();
    }

    public async Task Dispatch([NotNull] DashboardContext conterecurringJobt) 
    {
        using var activity = OTel.Application.StartActivity($"{nameof(GetJobForEdit)}.{nameof(Dispatch)}");

        var response = new Response() { Status = true };

        if (!"GET".Equals(conterecurringJobt.Request.Method, StringComparison.InvariantCultureIgnoreCase)) {
            conterecurringJobt.Response.StatusCode = 405;

            return;
        }

        var jobId = conterecurringJobt.Request.GetQuery("Id");
        var recurringJob = _connection.GetRecurringJobs().FirstOrDefault(x => x.Id == jobId);

        if (recurringJob == null) {
            response.Status = false;
            response.Message = "Job not found";

            await conterecurringJobt.Response.WriteAsync(JsonConvert.SerializeObject(response));

            return;
        }

        var periodicJob = new PeriodicJob {
            Id = recurringJob.Id,
            Cron = recurringJob.Cron,
            CreatedAt = recurringJob.CreatedAt,
            Error = recurringJob.Error,
            //                LastExecution = recurringJob.LastExecution,
            Method = recurringJob.Job.Method.Name,
            Class = recurringJob.Job.Method.ReflectedType.FullName,
            Queue = recurringJob.Queue,
            LastJobId = recurringJob.LastJobId,
            LastJobState = recurringJob.LastJobState,
            //                NextExecution = recurringJob.NextExecution,
            Removed = recurringJob.Removed,
            TimeZoneId = recurringJob.TimeZoneId
        };

        response.Object = periodicJob;

        await conterecurringJobt.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
