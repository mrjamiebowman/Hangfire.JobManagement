using Hangfire.Dashboard;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using Hangfire.Storage;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers
{
    internal class SettingsGetDispatcher : IDashboardDispatcher
    {
        // logger

        // hangfire
        private readonly IStorageConnection _connection;

        // repos
        private readonly ISettingsRepository _settingsRepository;

        public SettingsGetDispatcher(ISettingsRepository settingsRepository)
        {
            //ISettingsRepository
            _connection = JobStorage.Current.GetConnection();
            _settingsRepository = settingsRepository;
        }

        public async Task Dispatch(DashboardContext context)
        {
            if (!"GET".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.StatusCode = 405;
                return;
            }

            // data
            var data = await _settingsRepository.GetAsync();

            await context.Response.WriteAsync(JsonConvert.SerializeObject(data));
        }
    }
}
