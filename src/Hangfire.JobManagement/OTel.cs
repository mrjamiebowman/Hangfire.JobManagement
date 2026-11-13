using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Hangfire.JobManagement;

internal static class OTel
{
    public static string ApplicationName { get; set; } = "Hangfire.JobManagement";

    public static string ServiceVersion { get; set; } = "2.1.0";

    public static ActivitySource Application = new(ApplicationName);

    public static class Meters
    {
        public static Meter JobManagementMeter = new Meter(OTel.ApplicationName, OTel.ServiceVersion);

        public static string Namespace = "hangfire.jobmanagement";

        public static class MetricNames
        {
            public static string Notifications = $"{OTel.Meters.Namespace}.notifications";
        }

        private static Counter<int> ConsumedMessage = OTel.Meters.JobManagementMeter.CreateCounter<int>(OTel.Meters.MetricNames.Notifications, description: "A metric for everytime a Notification is triggered..");

        //public static void AddConsumedMessage() => ConsumedMessage.Add(1);
    }
}
