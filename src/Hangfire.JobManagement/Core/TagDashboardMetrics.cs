using Hangfire.Dashboard;

namespace Hangfire.JobManagement.Core
{
    public static class TagDashboardMetrics
    {
        public static readonly DashboardMetric JobsStoppedCount = new DashboardMetric("JobsStopped:count", razorPage => {
            return new Metric(JobAgent.GetAllJobStopped().Count);
        });
    }
}
