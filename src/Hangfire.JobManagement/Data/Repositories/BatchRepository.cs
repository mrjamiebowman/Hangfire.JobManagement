using Hangfire.JobManagement.Configuration;

namespace Hangfire.JobManagement.Data.Repositories
{
    internal class BatchRepository : BaseEfRepository
    {
        public BatchRepository(JobManagementConfiguration jobManagementConfiguration) : base(jobManagementConfiguration)
        {
        }
    }
}
