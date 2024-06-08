using Hangfire.JobManagement.Configuration;
using Hangfire.JobManagement.Data.Entities;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Data.Repositories
{
    internal class SettingsRepository : BaseEfRepository, ISettingsRepository
    {
        public SettingsRepository(JobManagementConfiguration jobManagementConfiguration) : base(jobManagementConfiguration)
        {

        }

        public Task<IEnumerable<Setting>> GetAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        } 

        public Task<Setting> GetByIdAsync(long id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteByIdAsync(long id, CancellationToken cancellationToken) { throw new NotImplementedException(); }

        public Task SaveAsync(Setting model, CancellationToken cancellationToken) { throw new NotImplementedException(); }

        public Task<GlobalSettings> GetCompositeAsync(CancellationToken cancellationToken) { throw new NotImplementedException(); }

    }
}
