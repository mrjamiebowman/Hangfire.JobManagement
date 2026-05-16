using Hangfire.JobManagement.Test.Jobs.Abstractions.Parameters;
using Hangfire.JobManagement.Test.Jobs.Interfaces;
using Hangfire.Server;

namespace Hangfire.JobManagement.Test.Jobs.Abstractions;

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
