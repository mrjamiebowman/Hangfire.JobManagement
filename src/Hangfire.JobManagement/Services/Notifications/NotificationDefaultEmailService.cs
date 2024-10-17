using Hangfire.JobManagement.Abstractions;
using Hangfire.JobManagement.Abstractions.Events;
using Hangfire.JobManagement.Abstractions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Services.Notifications;

public class NotificationDefaultEmailService : NotificationServiceBase, INotificationService
{
    public override string Name { get; set; } = "DefaultEmailService";

    //public Task ProcessEventAsync<T>(IEventNotification<T> @event, NotificationOptions notificationOptions, CancellationToken cancellation = default) where T : BaseEvent
    //{
    //    using var activity = OTel.Application.StartActivity($"{nameof(NotificationEmailService)}.{nameof(ProcessEventAsync)}");
    //    return Task.CompletedTask;
    //}
}
