using Hangfire.JobManagement.Abstractions.Events;

namespace Hangfire.JobManagement.Models.Notifications;

public class NotificationEvent<T> : IEventNotification where T : class
{

}
