using Hangfire.JobManagement.Configuration;

namespace Hangfire.JobManagement.Data.Repositories
{
    internal class JobHistoryRepository : BaseEfRepository
    {
        public JobHistoryRepository(JobManagementConfiguration jobManagementConfiguration) : base(jobManagementConfiguration)
        {
        }
    }
}
