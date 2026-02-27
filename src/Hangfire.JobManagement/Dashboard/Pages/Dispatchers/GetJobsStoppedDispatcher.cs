using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.JobManagement.Core;
using Hangfire.JobManagement.Models;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Dashboard.Pages.Dispatchers;

internal sealed class GetJobsStoppedDispatcher : IDashboardDispatcher
{
    // logger
    private readonly ILogger<GetJobsStoppedDispatcher> _logger;

    // hangfire
    private readonly IStorageConnection _connection;

    public GetJobsStoppedDispatcher(ILogger<GetJobsStoppedDispatcher> logger = null)
    {
        _logger = logger;
        _connection = JobStorage.Current.GetConnection();
    }

    public async Task Dispatch([NotNull] DashboardContext context) {
        using var activity = OTel.Application.StartActivity($"{nameof(GetJobsStoppedDispatcher)}.{nameof(Dispatch)}");

        if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase)) {
            context.Response.StatusCode = 405;

            return;
        }

        var periodicJob = new List<PeriodicJob>();
        periodicJob.AddRange(JobAgent.GetAllJobStopped());

        await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
    }
}
