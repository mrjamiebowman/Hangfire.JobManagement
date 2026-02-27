using Hangfire.Annotations;
using Hangfire.Dashboard;
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

    public GetTimeZonesDispatcher(ILogger<GetTimeZonesDispatcher> logger) => _logger = logger;

    public async Task Dispatch([NotNull] DashboardContext context) {
        using var activity = OTel.Application.StartActivity($"{nameof(GetTimeZonesDispatcher)}.{nameof(Dispatch)}");

        // get local time zone
        var localZone = TimeZoneInfo.Local;

        // get time zones
        var timeZones = Utility.GetTimeZones().ToList();

        // title
        var title = timeZones.SingleOrDefault(x => x.Item1 == localZone.StandardName)?.Item2 ?? localZone.StandardName;

        // set default time zone
        timeZones.Insert(0, new Tuple<string, string>(localZone.StandardName, title));

        await context.Response.WriteAsync(JsonConvert.SerializeObject(timeZones));
    }
}
