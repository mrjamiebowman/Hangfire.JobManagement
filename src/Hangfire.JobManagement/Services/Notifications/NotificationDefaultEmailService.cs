using Hangfire.JobManagement.Abstractions;
using Hangfire.JobManagement.Abstractions.Events;
using Hangfire.JobManagement.Abstractions.Notifications;
using Hangfire.JobManagement.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Services.Notifications
{
    public class NotificationDefaultEmailService : BaseNotificationService, INotificationService
    {
        public override string Name { get; set; } = "DefaultEmailService";

        public Task ProcessEventAsync<T>(NotificationEvent<T> @event, NotificationOptions notificationOptions, CancellationToken cancellation = default) where T : BaseEvent
        {
            using var activity = OTel.Application.StartActivity("NotificationEmailService.ProcessEventAsync");

            return Task.CompletedTask;
        }
    }
}
