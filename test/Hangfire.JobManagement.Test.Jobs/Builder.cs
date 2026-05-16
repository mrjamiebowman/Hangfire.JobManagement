using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

namespace Hangfire.JobManagement.Test.Jobs;

public static class OpenTelemetryExtensions
{
    public static TracerProviderBuilder AddHangfireTest(this TracerProviderBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.AddSource(OTel.Application.Name);
        return builder;
    }

    public static MeterProviderBuilder AddHangfireTest(this MeterProviderBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(builder);
        builder.AddMeter(OTel.Meters.JobManagementTestsMeter.Name);
        return builder;
    }
}
