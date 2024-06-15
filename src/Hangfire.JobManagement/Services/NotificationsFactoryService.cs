﻿using Hangfire.JobManagement.Abstractions;
using Hangfire.JobManagement.Abstractions.Events;
using Hangfire.JobManagement.Events;
using Hangfire.JobManagement.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Services;

public class NotificationsFactoryService : INotificationsFactoryService
{
    // logger

    // vars
    private List<INotificationService> _notificationServices  = new List<INotificationService>();

    public NotificationsFactoryService()
    {

    }

    public Task<List<INotificationService>> GetNotificationServicesAsync(CancellationToken cancellation = default)
    {
        // get notification services
        var serviceProvider = Builder.Services.BuildServiceProvider();
        _notificationServices = serviceProvider.GetServices<INotificationService>().ToList();

        return Task.FromResult(_notificationServices);
    }

    public async Task ProcessEventAsync<T>(NotificationEvent<T> @event, CancellationToken cancellation = default) where T : BaseEvent
    {
        // lookup notifications by the job or global
        var notificationServices = await GetNotificationServicesAsync();

        // process notifications
        foreach (var notificationService in notificationServices)
        {
            // process event
        }
    } 
}
