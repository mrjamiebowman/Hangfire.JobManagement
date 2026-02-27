using Hangfire.Dashboard;
using Hangfire.JobManagement.Models;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Dashboard.Pages.Dispatchers;

internal sealed class GetQueuesDispatcher : IDashboardDispatcher
{
    // logging
    private readonly ILogger<GetQueuesDispatcher> _logger;

    // hangfire
    private readonly IStorageConnection _connection;

    public GetQueuesDispatcher(ILogger<GetQueuesDispatcher> logger)
    {
        _logger = logger;
        _connection = JobStorage.Current.GetConnection();
    }

    public async Task Dispatch([NotNull] DashboardContext context)
    {
        using var activity = OTel.Application.StartActivity($"{nameof(GetJobForEdit)}.{nameof(Dispatch)}");

        var response = new Response() { Status = true };

        if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
        {
            context.Response.StatusCode = 405;
            return;
        }

        // TODO: improve... this only returns queues that are being used in current jobs.
        var monitoringApi = JobStorage.Current.GetMonitoringApi();
        var queues = monitoringApi.Queues();

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
