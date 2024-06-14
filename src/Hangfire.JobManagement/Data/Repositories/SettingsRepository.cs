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
    public class SettingsRepository : BaseEfRepository, ISettingsRepository
    {
        // logging
        private readonly ILogger<SettingsRepository> _logger;

        //ILogger<SettingsRepository> logger,
        public SettingsRepository(JobManagementDbFactory dbContextFactory, JobManagementConfiguration jobManagementConfiguration) : base(dbContextFactory, jobManagementConfiguration)
        {

        }

        public async Task<IEnumerable<Setting>> GetAsync(CancellationToken cancellationToken = default)
        {
            using var db = _dbContextFactory.Create();
            using (var trn = db.Database.BeginTransaction())
            {
                return await db.Settings.ToListAsync();
            }
        }

        public async Task<Setting> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            using var db = _dbContextFactory.Create();
            using (var trn = db.Database.BeginTransaction())
            {
                return await db.Settings.SingleOrDefaultAsync(x => x.SettingId == id);
            }
        }

        public async Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            using var db = _dbContextFactory.Create();

            using (var trn = db.Database.BeginTransaction())
            {
                // return await db.Settings.SingleOrDefaultAsync(x => x.SettingId == id);
                throw new NotImplementedException();
            }
        }
        public async Task<Setting> SaveAsync(Setting model, CancellationToken cancellationToken = default)
        {
            using var db = _dbContextFactory.Create();
            
            using (var trn = db.Database.BeginTransaction())
            {
                //return await db.Settings.SingleOrDefaultAsync(x => x.SettingId == id);
                throw new NotImplementedException();
            }
        }

        public async Task<GlobalSettings> GetCompositeAsync(CancellationToken cancellationToken = default)
        {
            using var db = _dbContextFactory.Create();

            using (var trn = db.Database.BeginTransaction())
            {
                //return await db.Settings.SingleOrDefaultAsync(x => x.SettingId == id);
                throw new NotImplementedException();
            }
        }

        public async Task<GlobalSettings> SaveAsync(GlobalSettings model, CancellationToken cancellationToken = default)
        {
            try
            {
                using var db = _dbContextFactory.Create();

                // save (DefaultTimeZoneId)
                var defaultTimeZoneId = model.DefaultTimeZoneId;
                var settingDefaultTimeZoneId = await db.Settings.SingleOrDefaultAsync(x => x.Name == nameof(GlobalSettings.DefaultTimeZoneId));
                settingDefaultTimeZoneId.Value = defaultTimeZoneId;

                // save (DefaultQueue)
                var defaultQueue = model.DefaultTimeZoneId;
                var settingDefaultQueue = await db.Settings.SingleOrDefaultAsync(x => x.Name == nameof(GlobalSettings.DefaultQueue));
                settingDefaultQueue.Value = defaultQueue;

                // save changes
                await db.SaveChangesAsync(cancellationToken);

                // TODO: reload
                return model;
            } catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
