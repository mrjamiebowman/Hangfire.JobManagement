using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Hangfire.JobManagement.Test.Jobs;

internal static class OTel
{
    public static string ApplicationName { get; set; } = "hangfire.jobmanagement.test.jobs";

    public static string ServiceVersion { get; set; } = "1.0.0";

    public static ActivitySource Application = new(ApplicationName);

    public static class Meters
    {
        public static Meter JobManagementTestsMeter = new Meter(OTel.ApplicationName, OTel.ServiceVersion);

        public static string Namespace = "jobmanagement.test";

        public static class MetricNames
        {
            public static string JobsRun = $"{OTel.Meters.Namespace}.jobs.run";
        }

        private static Counter<int> JobRun = OTel.Meters.JobManagementTestsMeter.CreateCounter<int>(OTel.Meters.MetricNames.JobsRun, description: "A metric for everytime a job is run.");

        public static void AddJobRun(TagList tagsList) => JobRun.Add(1, tagsList);
    }
}
