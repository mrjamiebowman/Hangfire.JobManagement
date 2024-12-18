﻿using Hangfire.Client;
using Hangfire.Common;
using Hangfire.JobManagement.Services.Interfaces;
using Hangfire.Logging;
using Hangfire.Server;
using Hangfire.States;
using Hangfire.Storage;

namespace Hangfire.JobManagement.Filters;

public class JobEventsFilter : JobFilterAttribute, IClientFilter, IServerFilter, IElectStateFilter, IApplyStateFilter
{
    // logger
    private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

    // notifications service
    private readonly INotificationsFactoryService _notificationsFactoryService;

    public JobEventsFilter(INotificationsFactoryService notificationsFactoryService)
    {
        _notificationsFactoryService = notificationsFactoryService;
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

        //// build event
        //var jobStatusEvent = new JobStatusEvent();
        //jobStatusEvent.JobName = context.Job.Method.Name;
        //jobStatusEvent.JobId = context.BackgroundJob?.Id;

        //// notification event
        //NotificationEvent<JobStatusEvent> @event = new NotificationEvent<JobStatusEvent>(jobStatusEvent);

        // process notifications
        //await _notificationsFactoryService.ProcessEventAsync(@event);
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
