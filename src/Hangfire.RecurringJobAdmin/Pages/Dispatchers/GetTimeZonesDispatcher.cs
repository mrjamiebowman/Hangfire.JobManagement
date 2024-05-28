using Hangfire.Annotations;
using Hangfire.JobManagement.Core;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers
{
    internal sealed class GetTimeZonesDispatcher : Dashboard.IDashboardDispatcher
    {
        public async Task Dispatch([NotNull] Dashboard.DashboardContext context) {
            await context.Response.WriteAsync(JsonConvert.SerializeObject(Utility.GetTimeZones()));
        }
    }
}
