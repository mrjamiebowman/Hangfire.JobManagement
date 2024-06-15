﻿using Hangfire.Dashboard;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using Hangfire.Storage;
using System;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers
{
    internal class SettingsQueueSaveDispatcher : IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;

        // repos
        private readonly ISettingsRepository _settingsRepository;

        public SettingsQueueSaveDispatcher(ISettingsRepository settingsRepository)
        {
            _connection = JobStorage.Current.GetConnection();
            _settingsRepository = settingsRepository;
        }

        public async Task Dispatch(DashboardContext context)
        {
            using var activity = OTel.Application.StartActivity("SettingsQueueSaveDispatcher.Dispatch");

            if (!"POST".Equals(context.Request.Method, StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.StatusCode = 405;
                return;
            }

            await Task.Delay(10);

            throw new NotImplementedException();

            //var periodicJob = new List<PeriodicJob>();
            //periodicJob.AddRange(JobAgent.GetAllJobStopped());

            //await context.Response.WriteAsync(JsonConvert.SerializeObject(periodicJob));
        }
    }
}
