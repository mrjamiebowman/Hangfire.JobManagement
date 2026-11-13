using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.JobManagement.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Dashboard.Pages.Dispatchers;

public class GetJobMethodsDispatchers : IDashboardDispatcher
{
    // logging
    private readonly ILogger<GetJobMethodsDispatchers> _logger;

    public GetJobMethodsDispatchers(ILogger<GetJobMethodsDispatchers> logger)
    {
        _logger = logger;
    }

    public async Task Dispatch([NotNull] DashboardContext context)
    {
        using var activity = OTel.Application.StartActivity($"{nameof(GetJobMethodsDispatchers)}.{nameof(Dispatch)}");

        var response = new Response() { Status = true };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
