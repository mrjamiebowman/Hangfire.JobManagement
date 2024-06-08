using Hangfire.JobManagement.Configuration;
using Hangfire.JobManagement.Data.Entities;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Data.Repositories
{
    internal class SettingsRepository : BaseEfRepository, ISettingsRepository
    {
        public SettingsRepository(JobManagementConfiguration jobManagementConfiguration) : base(jobManagementConfiguration)
        {

        }

        public async Task<IEnumerable<Setting>> GetAsync(CancellationToken cancellationToken = default)
        {
            using (var db = new JobManagementDbContext(_jobManagementConfiguration.ConnectionString))
            {
                using (var trn = db.Database.BeginTransaction())
                {
                    return await db.Settings.ToListAsync();
                }
            }
        } 

        public async Task<Setting> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            using (var db = new JobManagementDbContext(_jobManagementConfiguration.ConnectionString))
            {
                using (var trn = db.Database.BeginTransaction())
                {
                    return await db.Settings.SingleOrDefaultAsync(x => x.SettingId == id);
                }
            }
        }

        public async Task DeleteByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            using (var db = new JobManagementDbContext(_jobManagementConfiguration.ConnectionString))
            {
                using (var trn = db.Database.BeginTransaction())
                {
                    // return await db.Settings.SingleOrDefaultAsync(x => x.SettingId == id);
                    throw new NotImplementedException();
                }
            }
        }
        public async Task<Setting> SaveAsync(Setting model, CancellationToken cancellationToken = default) {
            using (var db = new JobManagementDbContext(_jobManagementConfiguration.ConnectionString))
            {
                using (var trn = db.Database.BeginTransaction())
                {
                    //return await db.Settings.SingleOrDefaultAsync(x => x.SettingId == id);
                    throw new NotImplementedException();
                }
            }
        }

        public async Task<GlobalSettings> GetCompositeAsync(CancellationToken cancellationToken = default) {
            using (var db = new JobManagementDbContext(_jobManagementConfiguration.ConnectionString))
            {
                using (var trn = db.Database.BeginTransaction())
                {
                    //return await db.Settings.SingleOrDefaultAsync(x => x.SettingId == id);
                    throw new NotImplementedException();
                }
            }
        }

        public Task<GlobalSettings> SaveAsync(GlobalSettings model, CancellationToken cancellationToken = default) {
            using (var db = new JobManagementDbContext(_jobManagementConfiguration.ConnectionString))
            {
                using (var trn = db.Database.BeginTransaction())
                {
                    //return await db.Settings.SingleOrDefaultAsync(x => x.SettingId == id);
                    throw new NotImplementedException();
                }
            }
        }
    }
}
