using Hangfire.Dashboard;

namespace Hangfire.RecurringJobAdmin.Core
{
    public static class TagDashboardMetrics
    {
        public static readonly DashboardMetric JobsStoppedCount = new DashboardMetric("JobsStopped:count", razorPage =>
            {
                return new Metric(JobAgent.GetAllJobStopped().Count);
            });
    }
}
