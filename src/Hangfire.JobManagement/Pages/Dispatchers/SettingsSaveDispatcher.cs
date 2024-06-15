using Hangfire.Dashboard;
using Hangfire.JobManagement.Data.Entities;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using Hangfire.JobManagement.Models;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Trace;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers
{
    internal class SettingsSaveDispatcher : IDashboardDispatcher
    {
        // logging
        private readonly ILogger<SettingsSaveDispatcher> _logger;

        // hangfire
        private readonly IStorageConnection _connection;

        // repos
        private readonly ISettingsRepository _settingsRepository;

        //ILogger<SettingsSaveDispatcher> logger, 
        public SettingsSaveDispatcher(ISettingsRepository settingsRepository)
        {
            _connection = JobStorage.Current.GetConnection();
            _settingsRepository = settingsRepository;
        }

        public async Task Dispatch(DashboardContext context)
        {
            using var activity = OTel.Application.StartActivity("SettingsSaveDispatcher.Dispatch");

            if (!"POST".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase)) {
                _logger.LogWarning($"Invalid POST.");
                activity?.SetStatus(ActivityStatusCode.Error, "Invalid POST");
                context.Response.StatusCode = 405;
                return;
            }

            try
            {
                // vars
                var globalSettings = new GlobalSetting();
                globalSettings.DefaultTimeZoneId = (await context.Request.GetFormValuesAsync("settings.DefaultTimeZoneId").ConfigureAwait(false)).FirstOrDefault();
                globalSettings.DefaultQueue = (await context.Request.GetFormValuesAsync("settings.DefaultQueue").ConfigureAwait(false)).FirstOrDefault();

                // save
                var data = await _settingsRepository.SaveAsync(globalSettings);

                // json
                var json = JsonSerializer.Serialize(data);

                await context.Response.WriteAsync(json);
            } catch (Exception ex) {
                _logger.LogError($"{ex.Message}", ex);
                activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
                activity?.RecordException(ex);
                throw ex;
            }
        }
    }
}
