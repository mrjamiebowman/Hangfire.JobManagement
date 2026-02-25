using Hangfire.JobManagement.Test.SampleServer.Jobs.Parameters;
using Hangfire.Server;

namespace Hangfire.JobManagement.Test.SampleServer.Interfaces;

public interface IJob
{
    public abstract Task ExecuteAsync(PerformContext context, JobParametersBase parameters, string title, CancellationToken cancellationToken);
}
