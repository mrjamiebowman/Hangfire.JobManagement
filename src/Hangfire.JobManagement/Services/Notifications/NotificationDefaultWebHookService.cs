using Hangfire.JobManagement.Abstractions;
using Hangfire.JobManagement.Abstractions.Events;
using Hangfire.JobManagement.Abstractions.Options;
using Hangfire.JobManagement.Models.Notifications;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Services.Notifications;

public class NotificationDefaultWebHookService : NotificationServiceBase, INotificationService
{
    public override string Name { get; set; } = "DefaultWebHooks";

    public Task ProcessEventAsync<T>(NotificationEvent<T> @event, NotificationOptions notificationOptions, CancellationToken cancellation = default) where T : BaseEvent
    {
        using var activity = OTel.Application.StartActivity($"{nameof(NotificationDefaultWebHookService)}.{nameof(ProcessEventAsync)}");

        return Task.CompletedTask;
    }
}
