using Hangfire.Dashboard;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using Hangfire.Storage;
using System;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers;

internal class SettingsQueueDeleteDispatcher : IDashboardDispatcher
{
    private readonly IStorageConnection _connection;

    // repos
    private readonly ISettingsRepository _settingsRepository;
    private readonly ISettingsQueueRepository _settingsQueueRepository;

    public SettingsQueueDeleteDispatcher(ISettingsRepository settingsRepository, ISettingsQueueRepository settingsQueueRepository)
    {
        _connection = JobStorage.Current.GetConnection();
        _settingsRepository = settingsRepository;
        _settingsQueueRepository = settingsQueueRepository;
    }

    public async Task Dispatch(DashboardContext context)
    {
        using var activity = OTel.Application.StartActivity("SettingsQueueDeleteDispatcher.Dispatch");

        if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase)) {
            context.Response.StatusCode = 405;
            return;
        }

        // vars
        var id = context.Request.GetQuery("id");
        var idInt = Convert.ToInt32(id);

        // delete
        await _settingsQueueRepository.DeleteAsync(idInt);

        //await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
        await context.Response.WriteAsync("OK");
    }
}
