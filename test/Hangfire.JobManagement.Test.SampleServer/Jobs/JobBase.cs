using Hangfire.JobManagement.Test.SampleServer.Interfaces;
using Hangfire.JobManagement.Test.SampleServer.Jobs.Parameters;
using Hangfire.Server;

namespace Hangfire.JobManagement.Test.SampleServer.Jobs;

public abstract class JobBase : IJob
{
    public CancellationToken GetCancellationToken(PerformContext context, CancellationToken? cancellationToken)
    {
        if (cancellationToken is null || cancellationToken == CancellationToken.None)
        {
            cancellationToken = context.CancellationToken.ShutdownToken;
        }

        return cancellationToken.Value;
    }

    public abstract Task ExecuteAsync(PerformContext context, JobParametersBase parameters, string title, CancellationToken cancellationToken);
}
