using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

namespace Hangfire.JobManagement.Jobs.Filters;

public class JobEventsFilter : JobFilterAttribute, IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
{
    // logger
    private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

    public JobEventsFilter()
    {

    }

    public void OnCreating(CreatingContext context)
    {
        using var activity = OTel.Application.StartActivity("JobEventsFilter.OnCreating");

        Logger.InfoFormat("Creating a job based on method `{0}`...", context.Job.Method.Name);
    }

    public void OnCreated(CreatedContext context)
    {
        using var activity = OTel.Application.StartActivity("JobEventsFilter.OnCreated");

        Logger.InfoFormat("Job that is based on method `{0}` has been created with id `{1}`", context.Job.Method.Name, context.BackgroundJob?.Id);
    }

    public void OnPerforming(PerformingContext context)
    {
        using var activity = OTel.Application.StartActivity("JobEventsFilter.OnPerforming");

        Logger.InfoFormat("Starting to perform job `{0}`", context.BackgroundJob.Id);
    }

    public void OnPerformed(PerformedContext context)
    {
        using var activity = OTel.Application.StartActivity("JobEventsFilter.OnPerformed");

        Logger.InfoFormat("Job `{0}` has been performed", context.BackgroundJob.Id);
    }

    public void OnStateElection(ElectStateContext context)
    {
        using var activity = OTel.Application.StartActivity("JobEventsFilter.OnStateElection");

        var failedState = context.CandidateState as FailedState;

        if (failedState != null)
        {
            Logger.WarnFormat(
                "Job `{0}` has been failed due to an exception `{1}`",
                context.BackgroundJob.Id,
                failedState.Exception);
        }
    }

    public void OnStateApplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        using var activity = OTel.Application.StartActivity("JobEventsFilter.OnStateApplied");

        Logger.InfoFormat(
            "Job `{0}` state was changed from `{1}` to `{2}`",
            context.BackgroundJob.Id,
            context.OldStateName,
            context.NewState.Name);
    }

    public void OnStateUnapplied(ApplyStateContext context, IWriteOnlyTransaction transaction)
    {
        using var activity = OTel.Application.StartActivity("JobEventsFilter.OnStateUnapplied");

        Logger.InfoFormat(
            "Job `{0}` state `{1}` was unapplied.",
            context.BackgroundJob.Id,
            context.OldStateName);
    }
}
