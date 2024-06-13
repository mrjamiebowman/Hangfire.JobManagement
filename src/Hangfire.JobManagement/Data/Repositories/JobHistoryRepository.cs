using Hangfire.JobManagement.Configuration;
using Microsoft.Extensions.Logging;

namespace Hangfire.JobManagement.Data.Repositories
{
    internal class JobHistoryRepository : BaseEfRepository
    {
        // logging
        private readonly ILogger<SettingsRepository> _logger;

        internal JobHistoryRepository(ILogger<JobHistoryRepository> logger, JobManagementDbFactory dbContextFactory, JobManagementConfiguration jobManagementConfiguration) : base(jobManagementConfiguration)
        {

        }
    }
}
