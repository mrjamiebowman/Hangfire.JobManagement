using Hangfire.Annotations;
using Hangfire.JobManagement.Core;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers
{
    internal sealed class GetTimeZonesDispatcher : Dashboard.IDashboardDispatcher
    {
        public async Task Dispatch([NotNull] Dashboard.DashboardContext context) {
            // get local time zone
            var localZone = TimeZone.CurrentTimeZone;

            // get time zones
            var timeZones = Utility.GetTimeZones().ToList();

            // title
            var title = timeZones.SingleOrDefault(x => x.Item1 == localZone.StandardName)?.Item2 ?? localZone.StandardName;

            // set default time zone
            timeZones.Insert(0, new Tuple<string, string>(localZone.StandardName, title));

            await context.Response.WriteAsync(JsonConvert.SerializeObject(timeZones));
        }
    }
}
