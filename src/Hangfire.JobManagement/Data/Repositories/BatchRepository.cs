using Hangfire.JobManagement.Configuration;
using Microsoft.Extensions.Logging;

namespace Hangfire.JobManagement.Data.Repositories
{
    internal class BatchRepository : BaseEfRepository
    {
        // logging
        private readonly ILogger<BatchRepository> _logger;

        internal BatchRepository(ILogger<BatchRepository> logger, JobManagementDbFactory dbContextFactory, JobManagementConfiguration jobManagementConfiguration) : base(dbContextFactory, jobManagementConfiguration)
        {

        }
    }
}
