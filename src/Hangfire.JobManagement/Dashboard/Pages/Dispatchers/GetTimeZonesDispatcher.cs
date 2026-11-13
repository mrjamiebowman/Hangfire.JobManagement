using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.JobManagement.Configuration;
using Hangfire.JobManagement.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Dashboard.Pages.Dispatchers;

internal sealed class GetTimeZonesDispatcher : IDashboardDispatcher
{
    // logging
    private readonly ILogger<GetTimeZonesDispatcher> _logger;

    // config
    private readonly JobManagementConfiguration _jobManagementConfiguration;

    public GetTimeZonesDispatcher(ILogger<GetTimeZonesDispatcher> logger, JobManagementConfiguration jobManagementConfiguration)
    {
        _logger = logger;
        _jobManagementConfiguration = jobManagementConfiguration;
    }

    public async Task Dispatch([NotNull] DashboardContext context) {
        using var activity = OTel.Application.StartActivity($"{nameof(GetTimeZonesDispatcher)}.{nameof(Dispatch)}");

        // vars
        var localZone = TimeZoneInfo.Local;
        var timeZones = Utility.GetTimeZones().ToList();
        var title = timeZones.FirstOrDefault(x => x.Item1 == localZone.Id)?.Item2 ?? localZone.Id;

        // list the server's own time zone first (by id, which is what jobs are saved with)
        timeZones.RemoveAll(x => x.Item1 == localZone.Id);
        timeZones.Insert(0, new Tuple<string, string>(localZone.Id, title));

        var data = new {
            timeZones = timeZones,
            defaultTimeZone = string.IsNullOrWhiteSpace(_jobManagementConfiguration.DefaultTimeZone) ? localZone.Id : _jobManagementConfiguration.DefaultTimeZone
        };

        // data
        var json = JsonConvert.SerializeObject(data);

        await context.Response.WriteAsync(json);
    }
}
