using Hangfire.JobManagement.Configuration;

namespace Hangfire.JobManagement.Data.Repositories
{
    internal abstract class BaseEfRepository
    {
        // configuration
        protected readonly JobManagementConfiguration _jobManagementConfiguration;

        // db context factory
        protected readonly JobManagementDbContextFactory _dbContextFactory;

        internal BaseEfRepository(JobManagementConfiguration jobManagementConfiguration, JobManagementDbContextFactory dbContextFactory)
        {
            _jobManagementConfiguration = jobManagementConfiguration;
            _dbContextFactory = dbContextFactory;
        }
    }
}
