using Hangfire.JobManagement.Abstractions.Events;
using Hangfire.JobManagement.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Services.Interfaces;

public interface INotificationsFactoryService
{
    Task ProcessEventAsync<T>(NotificationEvent<T> @event, CancellationToken cancellation = default) where T : BaseEvent;
}
