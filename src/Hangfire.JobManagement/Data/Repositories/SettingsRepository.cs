using Hangfire.JobManagement.Configuration;

namespace Hangfire.JobManagement.Data.Repositories
{
    internal class SettingsRepository : BaseEfRepository
    {
        public SettingsRepository(JobManagementConfiguration jobManagementConfiguration) : base(jobManagementConfiguration)
        {
        }
    }
}
