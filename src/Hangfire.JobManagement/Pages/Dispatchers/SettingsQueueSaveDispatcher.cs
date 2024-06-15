using Hangfire.Dashboard;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers
{
    internal class SettingsQueueSaveDispatcher : IDashboardDispatcher
    {
        // logging
        private readonly ILogger<SettingsSaveDispatcher> _logger;

        // hangfire
        private readonly IStorageConnection _connection;

        // repos
        private readonly ISettingsRepository _settingsRepository;
        private readonly ISettingsQueueRepository _settingsQueueRepository;

        public SettingsQueueSaveDispatcher(ISettingsRepository settingsRepository, ISettingsQueueRepository settingsQueueRepository)
        {
            _connection = JobStorage.Current.GetConnection();
            _settingsRepository = settingsRepository;
            _settingsQueueRepository = settingsQueueRepository;
        }

        public async Task Dispatch(DashboardContext context)
        {
            using var activity = OTel.Application.StartActivity("SettingsQueueSaveDispatcher.Dispatch");

            try
            {
                if (!"POST".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogWarning($"Invalid POST.");
                    activity?.SetStatus(ActivityStatusCode.Error, "Invalid POST");
                    context.Response.StatusCode = 405;
                    return;
                }

                // data
                var data = await _settingsQueueRepository.GetAsync();

                // response
                var json = JsonSerializer.Serialize(data);
                await context.Response.WriteAsync(json);
            } catch (Exception ex) {
                //activity?.RecordException(ex);
                throw ex;
            }
        }
    }
}
