using Hangfire.JobManagement.Abstractions.Events;
using Hangfire.JobManagement.Events;
using Hangfire.JobManagement.Services.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Services;

public class NotificationsFactoryService : INotificationsFactoryService
{
    public NotificationsFactoryService()
    {

    }

    // TODO: get notification services


    public Task ProcessEventAsync<T>(NotificationEvent<T> @event, CancellationToken cancellation = default) where T : BaseEvent
    {
        return Task.CompletedTask;
    } 
}
