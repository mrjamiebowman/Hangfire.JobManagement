using Hangfire.Annotations;
using Hangfire.JobManagement.Core;
using Hangfire.JobManagement.Models;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers;

internal sealed class GetJobsStoppedDispatcher : Dashboard.IDashboardDispatcher
{
    private readonly IStorageConnection _connection;

    public GetJobsStoppedDispatcher() {
        _connection = JobStorage.Current.GetConnection();
    }

    public async Task Dispatch([NotNull] Dashboard.DashboardContext context) {
        using var activity = OTel.Application.StartActivity($"{nameof(GetJobsStoppedDispatcher)}.{nameof(Dispatch)}");

        if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase)) {
            context.Response.StatusCode = 405;
            return;
        }

        var periodicJob = new List<PeriodicJob>();

        periodicJob.AddRange(JobAgent.GetAllJobStopped());

        // sort by id
        periodicJob = periodicJob.OrderBy(x => x.Id).ToList();

        await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
    }
}
