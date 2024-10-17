using Hangfire.Dashboard;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using Hangfire.JobManagement.Models;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers;

internal class SettingsGetDispatcher : IDashboardDispatcher
{
    // logger

    // hangfire
    private readonly IStorageConnection _connection;

    // repos
    private readonly ISettingsRepository _settingsRepository;

    public SettingsGetDispatcher(ISettingsRepository settingsRepository)
    {
        _connection = JobStorage.Current.GetConnection();

        _settingsRepository = settingsRepository;
    }

    public async Task Dispatch(DashboardContext context)
    {
        using var activity = OTel.Application.StartActivity("SettingsGetDispatcher.Dispatch");

        if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
        {
            context.Response.StatusCode = 405;
            return;
        }

        // data
        var data = (await _settingsRepository.GetAsync()).ToList();

        // global settings
        var globalSettings = new GlobalSetting(data);

        await context.Response.WriteAsync(JsonConvert.SerializeObject(globalSettings));
    }
}
