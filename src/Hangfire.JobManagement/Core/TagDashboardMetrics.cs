using Hangfire.Dashboard;

namespace Hangfire.JobManagement.Core;

public static class TagDashboardMetrics
{
    // running + stopped, shown on the Job Management navbar item
    public static readonly DashboardMetric JobsTotalCount = new DashboardMetric("JobsTotal:count", razorPage => {
        return new Metric(JobAgent.GetTotalJobCount());
    });

    public static readonly DashboardMetric JobsStoppedCount = new DashboardMetric("JobsStopped:count", razorPage => {
        return new Metric(JobAgent.GetStoppedJobCount());
    });
}
