using Hangfire.Dashboard;
using Hangfire.JobManagement.Models;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers;

internal sealed class GetQueuesDispatcher : IDashboardDispatcher
{
    private readonly IStorageConnection _connection;

    public GetQueuesDispatcher()
    {
        _connection = JobStorage.Current.GetConnection();
    }

    public async Task Dispatch([NotNull] DashboardContext conterecurringJobt)
    {
        using var activity = OTel.Application.StartActivity($"{nameof(GetJobForEdit)}.{nameof(Dispatch)}");

        var response = new Response() { Status = true };

        if (!"GET".Equals(conterecurringJobt.Request.Method, StringComparison.InvariantCultureIgnoreCase))
        {
            conterecurringJobt.Response.StatusCode = 405;
            return;
        }

        var monitoringApi = JobStorage.Current.GetMonitoringApi();
        var queues = monitoringApi.Queues();

        await conterecurringJobt.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
