using Hangfire.JobManagement.Configuration;
using Microsoft.Extensions.Logging;

namespace Hangfire.JobManagement.Data.Repositories;

internal class NotificationsRepository : BaseEfRepository
{
    // logging
    private readonly ILogger<NotificationsRepository> _logger;

    internal NotificationsRepository(ILogger<NotificationsRepository> logger, JobManagementDbFactory dbContextFactory, JobManagementConfiguration jobManagementConfiguration) : base(dbContextFactory, jobManagementConfiguration)
    {

    }
}
