﻿using Hangfire.Dashboard;
using Hangfire.Storage;
using System;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers
{
    internal class SettingsQueueDeleteDispatcher : IDashboardDispatcher
    {
        private readonly IStorageConnection _connection;

        public SettingsQueueDeleteDispatcher()
        {
            _connection = JobStorage.Current.GetConnection();
        }

        public async Task Dispatch(DashboardContext context)
        {
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
