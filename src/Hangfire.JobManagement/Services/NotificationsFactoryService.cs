using Hangfire.JobManagement.Abstractions;
using Hangfire.JobManagement.Abstractions.Events;
using Hangfire.JobManagement.Events;
using Hangfire.JobManagement.Services.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Services;

public class NotificationsFactoryService : INotificationsFactoryService
{
    private List<INotificationService> _notificationServices  = new List<INotificationService>();

    public Task<List<INotificationService>> GetNotificationServicesAsync(CancellationToken cancellation = default)
    {
        return Task.FromResult(_notificationServices);
    }

    public Task ProcessEventAsync<T>(NotificationEvent<T> @event, CancellationToken cancellation = default) where T : BaseEvent
    {
        // lookup notifications by the job or global

        // process notifications

        return Task.CompletedTask;
    } 
}
