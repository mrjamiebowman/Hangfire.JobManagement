using Hangfire.JobManagement.Configuration;
using Hangfire.JobManagement.Data.Entities;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
