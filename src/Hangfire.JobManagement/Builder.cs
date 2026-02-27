using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.JobManagement.Abstractions;
using Hangfire.JobManagement.Configuration;
using Hangfire.JobManagement.Core;
using Hangfire.JobManagement.Dashboard.Pages;
using Hangfire.JobManagement.Dashboard.Pages.Dispatchers;
using Hangfire.JobManagement.Data;
using Hangfire.JobManagement.Data.Repositories;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using Hangfire.JobManagement.Jobs.Filters;
using Hangfire.JobManagement.Services;
using Hangfire.JobManagement.Services.Interfaces;
using Hangfire.JobManagement.Services.Notifications;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Hangfire.JobManagement;

public static class Builder
{
    internal static IServiceCollection Services;

    internal static IConfiguration Configuration;

    public static TBuilder ConfigureJobManagement<TBuilder>(this TBuilder builder) where TBuilder : IHostApplicationBuilder
    {
        // configuration
        JobManagementConfiguration jobManagementConfiguration = new JobManagementConfiguration();
        builder.Configuration.GetSection(JobManagementConfiguration.Position).Bind(jobManagementConfiguration);
        builder.Services.AddSingleton<JobManagementConfiguration>(jobManagementConfiguration);

        // inject: dbcontext factory
        builder.Services.AddScoped<JobManagementDbFactory, JobManagementDbFactory>();

        // inject: factories
        builder.Services.AddScoped<IDesignTimeDbContextFactory<JobManagementDbContext>, JobManagementDbFactory>();
        builder.Services.AddScoped<INotificationsFactoryService, NotificationsFactoryService>();

        // services
        builder.Services.AddScoped<IBatchService, BatchService>();
        builder.Services.AddScoped<IJobHistoryService, JobHistoryService>();

        builder.Services.AddScoped<INotificationService, NotificationDefaultEmailService>();
        builder.Services.AddScoped<INotificationService, NotificationDefaultWebHookService>();

        // inject: repositories
        builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
        builder.Services.AddScoped<ISettingsQueueRepository, SettingsQueuesRepository>();

        return builder;
    }

    [PublicAPI]
    public static IGlobalConfiguration UseJobManagement<TBuilder>(this IGlobalConfiguration config, TBuilder builder, Action<JobManagementBuilder> jobManagementOptions = default) where TBuilder : IHostApplicationBuilder
    {
        // injected
        Services = builder.Services;
        Configuration = builder.Configuration;

        // service provider
        var serviceProdvider = Services.BuildServiceProvider();
        var notificationsFactory = serviceProdvider.GetRequiredService<INotificationsFactoryService>();

        // filters
        GlobalJobFilters.Filters.Add(new JobEventsFilter(notificationsFactory));

        // get all jobs
        PeriodicJobBuilder.GetAllJobs();

        // set up hangfire integration
        SetJobManagementHangfireIntegration();

        return config;
    }

    private static void SetJobManagementHangfireIntegration() 
    {
        var serviceProvider = Builder.Services.BuildServiceProvider();

        ISettingsRepository settingsRepository = serviceProvider.GetService<ISettingsRepository>();
        ISettingsQueueRepository settingsQueueRepository = serviceProvider.GetService<ISettingsQueueRepository>();

        // logging
        ILogger<GetJobsStoppedDispatcher> loggerGetJobsStoppedDispatcher = serviceProvider.GetService<ILogger<GetJobsStoppedDispatcher>>();
        ILogger<GetJobDispatcher> loggerGetJobDispatcher = serviceProvider.GetService<ILogger<GetJobDispatcher>>();
        ILogger<ChangeJobDispatcher> loggerChangeJobDispatcher = serviceProvider.GetService<ILogger<ChangeJobDispatcher>>();
        ILogger<GetJobForEdit> loggerGetJobForEdit = serviceProvider.GetService<ILogger<GetJobForEdit>>();
        ILogger<JobAgentDispatcher> loggerJobAgentDispatcher = serviceProvider.GetService<ILogger<JobAgentDispatcher>>();
        ILogger<GetTimeZonesDispatcher> loggerGetTimeZonesDispatcher = serviceProvider.GetService<ILogger<GetTimeZonesDispatcher>>();
        ILogger<GetQueuesDispatcher> loggerGetQueuesDispatcher = serviceProvider.GetService<ILogger<GetQueuesDispatcher>>();

        ILogger<SettingsGetDispatcher> loggerSettingsGetDispatcher = serviceProvider.GetService<ILogger<SettingsGetDispatcher>>();
        ILogger<SettingsSaveDispatcher> loggerSettingsSaveDispatcher = serviceProvider.GetService<ILogger<SettingsSaveDispatcher>>();

        ILogger<SettingsQueueGetDispatcher> loggerSettingsQueueGetDispatcher = serviceProvider.GetService<ILogger<SettingsQueueGetDispatcher>>();
        ILogger<SettingsQueueDeleteDispatcher> loggerSettingsQueueDeleteDispatcher = serviceProvider.GetService<ILogger<SettingsQueueDeleteDispatcher>>();
        ILogger<SettingsQueueSaveDispatcher> loggerSettingsQueueSaveDispatcher = serviceProvider.GetService<ILogger<SettingsQueueSaveDispatcher>>();

        // pages
        DashboardRoutes.Routes.AddRazorPage(Dashboard.Pages.JobManagement.PageRoute, x => new Dashboard.Pages.JobManagement());
        DashboardRoutes.Routes.AddRazorPage(JobsStoppedPage.PageRoute, x => new JobsStoppedPage());
        DashboardRoutes.Routes.AddRazorPage(SettingsPage.PageRoute, x => new SettingsPage());
        //DashboardRoutes.Routes.AddRazorPage(NotificationsPage.PageRoute, x => new NotificationsPage());

        // routes sidebar
        DashboardRoutes.Routes.Add("/jobs/GetJobsStopped", new GetJobsStoppedDispatcher(loggerGetJobsStoppedDispatcher));

        // routes
        DashboardRoutes.Routes.Add("/management/data/GetJobs", new GetJobDispatcher(loggerGetJobDispatcher));
        DashboardRoutes.Routes.Add("/management/data/UpdateJobs", new ChangeJobDispatcher(loggerChangeJobDispatcher));
        DashboardRoutes.Routes.Add("/management/data/GetJob", new GetJobForEdit(loggerGetJobForEdit));
        DashboardRoutes.Routes.Add("/management/data/JobAgent", new JobAgentDispatcher(loggerJobAgentDispatcher));
        DashboardRoutes.Routes.Add("/management/data/timezones", new GetTimeZonesDispatcher(loggerGetTimeZonesDispatcher));
        DashboardRoutes.Routes.Add("/management/data/queues", new GetQueuesDispatcher(loggerGetQueuesDispatcher));

        // dispatcher: settings
        DashboardRoutes.Routes.Add("/management/settings/all", new SettingsGetDispatcher(loggerSettingsGetDispatcher, settingsRepository));
        DashboardRoutes.Routes.Add("/management/settings/save", new SettingsSaveDispatcher(loggerSettingsSaveDispatcher, settingsRepository));
        // dispatcher: queues
        DashboardRoutes.Routes.Add("/management/settings/queues/all", new SettingsQueueGetDispatcher(loggerSettingsQueueGetDispatcher, settingsRepository, settingsQueueRepository));
        DashboardRoutes.Routes.Add("/management/settings/queues/delete", new SettingsQueueDeleteDispatcher(loggerSettingsQueueDeleteDispatcher, settingsRepository, settingsQueueRepository));
        DashboardRoutes.Routes.Add("/management/settings/queues/save", new SettingsQueueSaveDispatcher(loggerSettingsQueueSaveDispatcher, settingsRepository, settingsQueueRepository));

        // jobs stopped
        DashboardMetrics.AddMetric(TagDashboardMetrics.JobsStoppedCount);

        // sidebar
        JobsSidebarMenu.Items.Add(page => new MenuItem("Jobs Stopped", page.Url.To(JobsStoppedPage.PageRoute)) {
            Active = page.RequestPath.StartsWith(JobsStoppedPage.PageRoute),
            Metric = TagDashboardMetrics.JobsStoppedCount,
        });

        // navbar
        NavigationMenu.Items.Add(page => new MenuItem(Dashboard.Pages.JobManagement.Title, page.Url.To(Dashboard.Pages.JobManagement.PageRoute)) {
            Active = page.RequestPath.StartsWith(Dashboard.Pages.JobManagement.PageRoute),
            Metric = DashboardMetrics.RecurringJobCount
        });


        //// notifications
        //if (Builder.Options.Features.Notifications) 
        //{
        //    NavigationMenu.Items.Add(page => new MenuItem(NotificationsPage.Title, page.Url.To(NotificationsPage.PageRoute)) {
        //        Active = page.RequestPath.StartsWith(NotificationsPage.PageRoute)
        //    });
        //}

        //// settings
        //if (Builder.Options.Features.Settings)
        //{
        //    NavigationMenu.Items.Add(page => new MenuItem(SettingsPage.Title, page.Url.To(SettingsPage.PageRoute)) {
        //        Active = page.RequestPath.StartsWith(SettingsPage.PageRoute)
        //    });
        //}



        // swagger link
//        DashboardRoutes.Routes.Add("/custom.js", new EmbeddedJsDispatcher(@"
//    document.addEventListener('DOMContentLoaded', function() {
//        var navbar = document.querySelector('.navbar-right');
//        if (navbar) {
//            navbar.insertAdjacentHTML('beforeend',
//                '<li><a href=""/my-link"">My Item</a></li>'
//            );
//        }
//    });
//"));

        // css 
        AddDashboardRouteToEmbeddedResource("/resources/css/jobmanagement", "text/css", "Hangfire.JobManagement.Dashboard.Content.css.jobmanagement.css");
        AddDashboardRouteToEmbeddedResource("/resources/css/cron-input-ui", "text/css", "Hangfire.JobManagement.Dashboard.Content.css.cron-input-ui.css");

        // js
        AddDashboardRouteToEmbeddedResource("/resources/js/page", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.jobextension.js");
        AddDashboardRouteToEmbeddedResource("/resources/js/vue", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.vue.js");
        AddDashboardRouteToEmbeddedResource("/resources/js/vue3", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.vue.3.4.27.js");
        AddDashboardRouteToEmbeddedResource("/resources/js/axio", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.axios.min.js");
        AddDashboardRouteToEmbeddedResource("/resources/js/daysjs", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.daysjs.min.js");
        AddDashboardRouteToEmbeddedResource("/resources/js/relativeTime", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.relativeTime.min.js");
        AddDashboardRouteToEmbeddedResource("/resources/js/vuejsPaginate", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.vuejs-paginate.js");
        AddDashboardRouteToEmbeddedResource("/resources/js/sweetalert", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.sweetalert.js");
        AddDashboardRouteToEmbeddedResource("/resources/js/cron-input-ui", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.cron-input-ui.js");
    }

    private static void AddDashboardRouteToEmbeddedResource(string route, string contentType, string resourceName)
       => DashboardRoutes.Routes.Add(route, new ContentDispatcher(contentType, resourceName, TimeSpan.FromDays(1)));
}

public static class JobManagementBuilderExtensions
{
    public static JobManagementBuilder ConfigureAssemblies(this JobManagementBuilder builder, [NotNull] params Assembly[] assemblies)
    {
        builder.ValidateConfiguration();
        if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));
        StorageAssemblySingleton.GetInstance().SetCurrentAssembly(assemblies: assemblies);
        return builder;
    }

    public static JobManagementBuilder ConfigureDatabase(this JobManagementBuilder builder)
    {
        return builder;
    }

    public static JobManagementBuilder ConfigureFeatures(this JobManagementBuilder builder, Action<JobManagementFeatures> features)
    {
        features.Invoke(builder.Features);
        return builder;
    }

    internal static JobManagementBuilder ConfigureDefaultServices(this JobManagementBuilder builder)
    {
        builder.ValidateConfiguration();

        return builder;
    }

    internal static JobManagementBuilder ValidateConfiguration(this JobManagementBuilder builder)
    {
        if (Builder.Configuration is null) throw new ArgumentNullException($"Please call SetConfiguration() first. Argument Null: {nameof(Builder.Configuration)}");
        return builder;
    }
}
