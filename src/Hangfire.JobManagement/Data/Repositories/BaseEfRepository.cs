using Hangfire.JobManagement.Configuration;

namespace Hangfire.JobManagement.Data.Repositories
{
    internal abstract class BaseEfRepository
    {
        // dbcontext
        protected readonly JobManagementDbFactory _dbContextFactory;

        // configuration
        protected readonly JobManagementConfiguration _jobManagementConfiguration;

        internal BaseEfRepository(JobManagementDbFactory dbContextFactory, JobManagementConfiguration jobManagementConfiguration)
        {
            _dbContextFactory = dbContextFactory;
            _jobManagementConfiguration = jobManagementConfiguration;
        }
    }
}
