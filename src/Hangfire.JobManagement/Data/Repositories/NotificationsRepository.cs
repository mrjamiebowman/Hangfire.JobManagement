using Hangfire.JobManagement.Configuration;

namespace Hangfire.JobManagement.Data.Repositories
{
    internal class NotificationsRepository : BaseEfRepository
    {
        public NotificationsRepository(JobManagementConfiguration jobManagementConfiguration) : base(jobManagementConfiguration)
        {
        }
    }
}
