using Hangfire.JobManagement.Configuration;

namespace Hangfire.JobManagement.Data.Repositories
{
    public abstract class BaseEfRepository
    {
        // dbcontext
        protected readonly JobManagementDbFactory _dbContextFactory;

        // configuration
        protected readonly JobManagementConfiguration _jobManagementConfiguration;

        public BaseEfRepository(JobManagementDbFactory dbContextFactory, JobManagementConfiguration jobManagementConfiguration)
        {
            _dbContextFactory = dbContextFactory;
            _jobManagementConfiguration = jobManagementConfiguration;
        }
    }
}
