using System.Diagnostics.Metrics;
using System.Diagnostics;

namespace Hangfire.JobManagement;

internal static class OTel
{
    public static string ApplicationName { get; set; } = "Hangfire.JobManagement";

    public static string ServiceVersion { get; set; } = "2.1.0";


    public static ActivitySource Application = new(ApplicationName);

    public static class Spans
    {
        #region user

        public const string UserName = "user.username";

        public const string UserId = "user.id";

        #endregion
    }

    public static class Meters
    {
        public static class Application
        {
            public static string ServiceVersion { get; set; } = "1.0.0";

            public static Meter ApplicationMeter = new Meter(OTel.ApplicationName, OTel.ServiceVersion);

            public static string Namespace = "hangfire.jobmanagement";

            public static class MetricNames
            {
                public static string Notifications = $"{OTel.Meters.Application.Namespace}.notifications";
            }

            public static class Meters
            {
                private static Counter<int> ConsumedMessage = OTel.Meters.Application.ApplicationMeter.CreateCounter<int>(OTel.Meters.Application.MetricNames.Notifications, description: "A metric for everytime a Notification is triggered..");

                public static void AddConsumedMessage() => ConsumedMessage.Add(1);
            }
        }
    }
}
