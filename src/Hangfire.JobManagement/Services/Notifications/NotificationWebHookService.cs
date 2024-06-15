using Hangfire.JobManagement.Abstractions;
using Hangfire.JobManagement.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Services.Notifications
{
    public class NotificationWebHookService : INotificationService
    {
        public Task ProcessEventAsync(NotificationEvent @event, CancellationToken cancellation = default) => throw new System.NotImplementedException();
    }
}
