using Hangfire.Dashboard;
using Hangfire.JobManagement.Data.Entities;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using Hangfire.Storage;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers
{
    internal class SettingsSaveDispatcher : IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;

        private readonly ISettingsRepository _settingsRepository;

        public SettingsSaveDispatcher(ISettingsRepository settingsRepository)
        {
            _connection = JobStorage.Current.GetConnection();
            _settingsRepository = settingsRepository;
        }

        public async Task Dispatch(DashboardContext context)
        {
            if (!"POST".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase)) {
                context.Response.StatusCode = 405;
                return;
            }

            // vars
            var globalSettings = new GlobalSettings();
            globalSettings.DefaultTimeZoneId = (await context.Request.GetFormValuesAsync("settings.DefaultTimeZoneId").ConfigureAwait(false)).FirstOrDefault();
            //globalSettings.DefaultQueue = (await context.Request.GetFormValuesAsync("settings.DefaultQueue").ConfigureAwait(false)).FirstOrDefault();

            await Task.Delay(10);

            throw new NotImplementedException();

            //var periodicJob = new List<PeriodicJob>();
            //periodicJob.AddRange(JobAgent.GetAllJobStopped());

            //await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
        }
    }
}
