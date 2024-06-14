using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.JobManagement.Configuration;
using Hangfire.JobManagement.Core;
using Hangfire.JobManagement.Data;
using Hangfire.JobManagement.Data.Repositories;
using Hangfire.JobManagement.Data.Repositories.Interfaces;
using Hangfire.JobManagement.Pages;
using Hangfire.JobManagement.Pages.Dispatchers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Hangfire.JobManagement
{
    public class JobManagementFeatures
    {
        public bool Notifications { get; set; } = false;

        public bool Settings { get; set; } = true;
    }

    public class JobManagementBuilder 
    {
        public Assembly[] Assemblies { get; set;  }

        public JobManagementConfiguration Settings { get; set; } = new JobManagementConfiguration();

        public JobManagementFeatures Features { get; set; } = new JobManagementFeatures();

        public JobManagementBuilder(IConfiguration configuration)
        {
            // configure default services
            this.ConfigureDefaultServices();
        }
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

    public static class Builder
    {
        private static JobManagementBuilder Options { get; set; }

        internal static IServiceCollection Services;

        internal static IConfiguration Configuration;

        internal static IServiceProvider GetServiceProvider()
        {
            return Services?.BuildServiceProvider();
        }

        [PublicAPI]
        public static IGlobalConfiguration UseJobManagement(this IGlobalConfiguration config, IServiceCollection services, IConfiguration configuration, Action<JobManagementBuilder> jobManagementOptions)
        {
            // injected
            Services = services;
            Configuration = configuration;

            // instantiate builder 
            Options = new JobManagementBuilder(configuration);

            // customization
            jobManagementOptions.Invoke(Options);

            PeriodicJobBuilder.GetAllJobs();

            CreateJobManagement();

            return config;
        }

        public static IServiceCollection ConfigureJobManagement(this IServiceCollection services, IConfiguration configuration)
        {
            // configuration
            JobManagementConfiguration jobManagementConfiguration = new JobManagementConfiguration();
            configuration.GetSection(JobManagementConfiguration.Position).Bind(jobManagementConfiguration);
            services.AddSingleton<JobManagementConfiguration>(jobManagementConfiguration);

            // inject: dbcontext factory
            services.AddTransient<JobManagementDbFactory, JobManagementDbFactory>();

            // inject: repositories
            services.AddTransient<ISettingsRepository, SettingsRepository>();

            return services;
        }

        // entity framework

        // open telemetry

        //public static IGlobalConfiguration UseJobManagement(this IGlobalConfiguration config, [NotNull] params string[] assemblies) {
        //    if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

        //    StorageAssemblySingleton.GetInstance().SetCurrentAssembly(assemblies: assemblies.Select(x => Type.GetType(x).Assembly).ToArray());

        //public static IGlobalConfiguration UseJobManagement(this IGlobalConfiguration config, bool includeReferences = false, [NotNull] params string[] assemblies) {

        ///// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        ///// <param name="assembliess"></param>
        //[PublicAPI]
        //public static IGlobalConfiguration UseJobManagement(this IGlobalConfiguration config, bool includeReferences = false, [NotNull] params Assembly[] assemblies) {
        //    if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

        //    StorageAssemblySingleton.GetInstance().SetCurrentAssembly(includeReferences, assemblies);

        private static void CreateJobManagement() {
            // di
            var serviceProvider = Builder.Services.BuildServiceProvider();

            ISettingsRepository settingsRepository = serviceProvider.GetService<ISettingsRepository>();

            // pages
            DashboardRoutes.Routes.AddRazorPage(Pages.JobManagement.PageRoute, x => new Pages.JobManagement());
            DashboardRoutes.Routes.AddRazorPage(JobsStoppedPage.PageRoute, x => new JobsStoppedPage());
            DashboardRoutes.Routes.AddRazorPage(SettingsPage.PageRoute, x => new SettingsPage());
            DashboardRoutes.Routes.AddRazorPage(NotificationsPage.PageRoute, x => new NotificationsPage());

            // routes sidebar
            DashboardRoutes.Routes.Add("/jobs/GetJobsStopped", new GetJobsStoppedDispatcher());

            // routes
            DashboardRoutes.Routes.Add("/management/data/GetJobs", new GetJobDispatcher());
            DashboardRoutes.Routes.Add("/management/data/UpdateJobs", new ChangeJobDispatcher());
            DashboardRoutes.Routes.Add("/management/data/GetJob", new GetJobForEdit());
            DashboardRoutes.Routes.Add("/management/data/JobAgent", new JobAgentDispatcher());
            DashboardRoutes.Routes.Add("/management/data/timezones", new GetTimeZonesDispatcher());

            // dispatcher: settings
            DashboardRoutes.Routes.Add("/management/settings/all", new SettingsGetDispatcher());
            DashboardRoutes.Routes.Add("/management/settings/queue/delete", new SettingsQueueDeleteDispatcher());
            DashboardRoutes.Routes.Add("/management/settings/queue/save", new SettingsQueueSaveDispatcher());
            DashboardRoutes.Routes.Add("/management/settings/save", new SettingsSaveDispatcher(settingsRepository)); //serviceProvider.GetService<ILogger<SettingsSaveDispatcher>>()

            // jobs stopped
            DashboardMetrics.AddMetric(TagDashboardMetrics.JobsStoppedCount);

            // sidebar
            JobsSidebarMenu.Items.Add(page => new MenuItem("Jobs Stopped", page.Url.To(JobsStoppedPage.PageRoute)) {
                Active = page.RequestPath.StartsWith(JobsStoppedPage.PageRoute),
                Metric = TagDashboardMetrics.JobsStoppedCount,
            });

            // navbar
            NavigationMenu.Items.Add(page => new MenuItem(Pages.JobManagement.Title, page.Url.To(Pages.JobManagement.PageRoute)) {
                Active = page.RequestPath.StartsWith(Pages.JobManagement.PageRoute),
                Metric = DashboardMetrics.RecurringJobCount
            });

            // notifications
            if (Builder.Options.Features.Notifications) 
            {
                NavigationMenu.Items.Add(page => new MenuItem(NotificationsPage.Title, page.Url.To(NotificationsPage.PageRoute)) {
                    Active = page.RequestPath.StartsWith(NotificationsPage.PageRoute)
                });
            }

            // settings
            if (Builder.Options.Features.Settings)
            {
                NavigationMenu.Items.Add(page => new MenuItem(SettingsPage.Title, page.Url.To(SettingsPage.PageRoute)) {
                    Active = page.RequestPath.StartsWith(SettingsPage.PageRoute)
                });
            }

            // css 
            AddDashboardRouteToEmbeddedResource("/resources/css/jobExtension", "text/css", "Hangfire.JobManagement.Dashboard.Content.css.JobExtension.css");
            AddDashboardRouteToEmbeddedResource("/resources/css/cron-expression-input", "text/css", "Hangfire.JobManagement.Dashboard.Content.css.cron-expression-input.css");

            // js
            AddDashboardRouteToEmbeddedResource("/resources/js/page", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.jobextension.js");
            AddDashboardRouteToEmbeddedResource("/resources/js/vue", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.vue.js");
            AddDashboardRouteToEmbeddedResource("/resources/js/vue3", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.vue.3.4.27.js");
            AddDashboardRouteToEmbeddedResource("/resources/js/axio", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.axios.min.js");
            AddDashboardRouteToEmbeddedResource("/resources/js/daysjs", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.daysjs.min.js");
            AddDashboardRouteToEmbeddedResource("/resources/js/relativeTime", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.relativeTime.min.js");
            AddDashboardRouteToEmbeddedResource("/resources/js/vuejsPaginate", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.vuejs-paginate.js");
            AddDashboardRouteToEmbeddedResource("/resources/js/sweetalert", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.sweetalert.js");
            AddDashboardRouteToEmbeddedResource("/resources/js/cron-expression-input", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.cron-expression-input.js");
        }

        private static void AddDashboardRouteToEmbeddedResource(string route, string contentType, string resourceName)
           => DashboardRoutes.Routes.Add(route, new ContentDispatcher(contentType, resourceName, TimeSpan.FromDays(1)));
    }
}
