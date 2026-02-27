using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.JobManagement.Core;
using Hangfire.JobManagement.Models;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Dashboard.Pages.Dispatchers;

internal sealed class ChangeJobDispatcher : IDashboardDispatcher
{
    // logging
    private readonly ILogger<ChangeJobDispatcher> _logger;

    // hangfire
    private readonly IStorageConnection _connection;
    private readonly RecurringJobRegistry _recurringJobRegistry;

    public ChangeJobDispatcher(ILogger<ChangeJobDispatcher> logger) {
        _logger = logger;
        _connection = JobStorage.Current.GetConnection();
        _recurringJobRegistry = new RecurringJobRegistry();
    }

    public async Task Dispatch([NotNull] DashboardContext context) {
        using var activity = OTel.Application.StartActivity($"{nameof(ChangeJobDispatcher)}.{nameof(Dispatch)}");

        var response = new Response() { Status = true };

        var job = new PeriodicJob();
        job.Id = context.Request.GetQuery("Id");
        job.Cron = context.Request.GetQuery("Cron");
        job.Class = context.Request.GetQuery("Class");
        job.Method = context.Request.GetQuery("Method");
        job.Queue = context.Request.GetQuery("Queue");
        job.TimeZoneId = context.Request.GetQuery("TimeZoneId");

        var timeZone = TimeZoneInfo.Utc;

        if (!Utility.IsValidSchedule(job.Cron)) {
            response.Status = false;
            response.Message = "Invalid CRON";

            _logger.LogWarning("{className}.{methodName}, Error: {error}",
                nameof(ChangeJobDispatcher),
                nameof(Dispatch),
                response.Message
            );

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

            return;
        }

        try {
            if (!string.IsNullOrEmpty(job.TimeZoneId)) {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(job.TimeZoneId);
            }
        } catch (Exception ex) {
            response.Status = false;
            response.Message = ex.Message;

            _logger.LogError("{className}.{methodName}, Error: {error}",
                nameof(ChangeJobDispatcher),
                nameof(Dispatch),
                ex.Message
            );

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

            return;
        }

        // verify class
        if (!StorageAssemblySingleton.GetInstance().IsValidType(job.Class)) {
            response.Status = false;
            response.Message = "The Class not found";

            _logger.LogWarning("{className}.{methodName}, Job Class not found. (Job Class: {jobClass})",
                nameof(ChangeJobDispatcher),
                nameof(Dispatch),
                job.Class
            );

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

            return;
        }

        // verify method
        if (!StorageAssemblySingleton.GetInstance().IsValidMethod(job.Class, job.Method)) {
            response.Status = false;
            response.Message = "The Method not found";

            _logger.LogWarning("{className}.{methodName}, Job Method not found. (Job Method: {jobMethod}) on class (Job Class: {jobClass}).",
                nameof(ChangeJobDispatcher),
                nameof(Dispatch),
                job.Method,
                job.Class
            );

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));

            return;
        }

        var methodInfo = StorageAssemblySingleton.GetInstance().currentAssembly
                                                                            .Where(x => x?.GetType(job.Class)?.GetMethod(job.Method) != null)
                                                                            .FirstOrDefault()
                                                                            .GetType(job.Class)
                                                                            .GetMethod(job.Method);

        _recurringJobRegistry.Register(
                  job.Id,
                  methodInfo,
                  job.Cron,
                  timeZone,
                  job.Queue ?? EnqueuedState.DefaultQueue);


        context.Response.StatusCode = (int)HttpStatusCode.OK;

        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
    }
}
