using Hangfire.JobManagement.Test.SampleServer.Jobs;

namespace Hangfire.JobManagement.Test.SampleServer;

public static class Builder
{
    public static TBuilder ConfigureJobManager<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        // jobs
        builder.Services.AddScoped<CustomRecurringJob>();

        return builder;
    }
}
