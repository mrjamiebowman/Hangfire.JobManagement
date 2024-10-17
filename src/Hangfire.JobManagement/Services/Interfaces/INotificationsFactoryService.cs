using Hangfire.JobManagement.Abstractions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Services.Interfaces;

public interface INotificationsFactoryService
{
    Task<List<INotificationService>> GetNotificationServicesAsync(CancellationToken cancellation = default);

    //Task ProcessEventAsync<T>(NotificationEvent<T> @event, CancellationToken cancellation = default) where T : BaseEvent;
}
