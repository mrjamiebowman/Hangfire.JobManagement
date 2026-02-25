using Serilog;

namespace Hangfire.JobManagement.Test.SampleServer;

public class HangfireJobActivator : JobActivator
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="HangfireJobActivator"/> class.
    /// </summary>
    /// <param name="serviceCollection">The service collection.</param>
    public HangfireJobActivator(IServiceCollection serviceCollection)
    {
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    public override JobActivatorScope BeginScope(JobActivatorContext context)
    {
        var scope = _serviceProvider.CreateScope();
        return new HangfireJobActivatorScope(scope);
    }

    /// <summary>
    /// Activates the job.
    /// </summary>
    /// <param name="jobType">Type of the job.</param>
    /// <returns>object</returns>
    public override object ActivateJob(Type jobType)
    {
        try
        {
            // get service
            var service = _serviceProvider.GetService(jobType);

            if (service == null)
            {
                Log.Information($"{jobType.FullName} not found.");
                throw new InvalidOperationException($"Unable to resolve service for type {jobType.FullName}");
            }

            return service;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private class HangfireJobActivatorScope : JobActivatorScope
    {
        private readonly IServiceScope _scope;

        public HangfireJobActivatorScope(IServiceScope scope)
        {
            _scope = scope;
        }

        public override object Resolve(Type type)
        {
            // get service
            var service = _scope.ServiceProvider.GetService(type);

            if (service == null)
            {
                Log.Information($"{type.FullName} not found.");
                throw new InvalidOperationException($"Unable to resolve service for type {type.FullName}");
            }

            return service;
        }

        public override void DisposeScope()
        {
            _scope.Dispose();
        }
    }
}
