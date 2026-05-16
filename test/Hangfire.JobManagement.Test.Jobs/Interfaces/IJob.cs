using Hangfire.JobManagement.Test.Jobs.Abstractions.Parameters;
using Hangfire.Server;

namespace Hangfire.JobManagement.Test.Jobs.Interfaces;

public interface IJob
{
    public abstract Task ExecuteAsync(PerformContext context, JobParametersBase parameters, string title, CancellationToken cancellationToken);
}
