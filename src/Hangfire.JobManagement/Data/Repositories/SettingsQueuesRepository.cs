using Hangfire.JobManagement.Configuration;
using Hangfire.JobManagement.Data.Entities;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Data.Repositories
{
    public class SettingsQueuesRepository : BaseEfRepository, ISettingsQueueRepository
    {
        // logging
        private readonly ILogger<SettingsQueuesRepository> _logger;

        public SettingsQueuesRepository(JobManagementDbFactory dbContextFactory, JobManagementConfiguration jobManagementConfiguration) : base(dbContextFactory, jobManagementConfiguration)
        {

        }

        public async Task DeleteByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            using var db = _dbContextFactory.Create();

            var model = await db.SettingsQueues.SingleOrDefaultAsync(x => x.SettingQueueId == id);

            if (model == null) {
                // TODO: log and return
                return;
            }

            // remove
            db.SettingsQueues.Remove(model);
        }

        public async Task<SettingQueue> SaveAsync(SettingQueue model, CancellationToken cancellationToken = default)
        {
            try
            {
                using var db = _dbContextFactory.Create();

                // save
                db.SettingsQueues.Update(model);

                // save changes
                await db.SaveChangesAsync(cancellationToken);

                // TODO: reload
                return model;
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<SettingQueue>> GetAsync(CancellationToken cancellationToken = default)
        {
            using var db = _dbContextFactory.Create();
            using (var trn = db.Database.BeginTransaction())
            {
                return await db.SettingsQueues.ToListAsync();
            }
        }
    }
}
