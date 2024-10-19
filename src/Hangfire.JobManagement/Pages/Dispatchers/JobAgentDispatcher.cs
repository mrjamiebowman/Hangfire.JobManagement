using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.JobManagement.Core;
using Hangfire.JobManagement.Models;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Pages.Dispatchers;

internal sealed class JobAgentDispatcher : IDashboardDispatcher
{
    public async Task Dispatch([NotNull] DashboardContext context) {
        using var activity = OTel.Application.StartActivity($"{nameof(JobAgentDispatcher)}.{nameof(Dispatch)}");

        var response = new Response() { Status = true };

        var jobId = context.Request.GetQuery("Id");
        var action = context.Request.GetQuery("Action");

        if (!JobAgent.IsValidJobId(jobId)) {
            response.Status = false;
            response.Message = $"The Job Id {jobId} not found";

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

            return;
        }

        if ("Stop".Equals(action)) {
            JobAgent.StopBackgroundJob(jobId);
        }

        if ("Start".Equals(action)) {
            JobAgent.StartBackgroundJob(jobId);
        }

        context.Response.StatusCode = (int)HttpStatusCode.OK;

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
