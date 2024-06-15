using Hangfire.JobManagement.Abstractions;
using Hangfire.JobManagement.Abstractions.Events;
using Hangfire.JobManagement.Abstractions.Notifications;
using Hangfire.JobManagement.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Services.Notifications
{
    public class NotificationWebHookService : BaseNotificationService, INotificationService
    {
        public override string Name { get; set; } = "DefaultWebHooks";

        public Task ProcessEventAsync<T>(NotificationEvent<T> @event, NotificationOptions notificationOptions, CancellationToken cancellation = default) where T : BaseEvent
        {
            using var activity = OTel.Application.StartActivity("NotificationWebHookService.ProcessEventAsync");

            return Task.CompletedTask;
        }
    }
}
