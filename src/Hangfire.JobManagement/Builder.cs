using Hangfire.Annotations;
using Hangfire.Dashboard;
using Hangfire.JobManagement.Core;
using Hangfire.JobManagement.Pages;
using System;
using System.Linq;
using System.Reflection;
using Hangfire.JobManagement.Pages.Dispatchers;

namespace Hangfire.JobManagement
{
    public static class Builder
    {
        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assemblies"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseJobManagement(this IGlobalConfiguration config, [NotNull] params string[] assemblies) {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(assemblies: assemblies.Select(x => Type.GetType(x).Assembly).ToArray());
            PeriodicJobBuilder.GetAllJobs();
            CreateManagementJob();
            return config;
        }

        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assemblies"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseJobManagement(this IGlobalConfiguration config, bool includeReferences = false, [NotNull] params string[] assemblies) {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(includeReferences, assemblies.Select(x => Type.GetType(x).Assembly).ToArray());
            PeriodicJobBuilder.GetAllJobs();
            CreateManagementJob();
            return config;
        }

        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assemblies"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseJobManagement(this IGlobalConfiguration config, [NotNull] params Assembly[] assemblies) {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(assemblies: assemblies);
            PeriodicJobBuilder.GetAllJobs();
            CreateManagementJob();
            return config;
        }

        /// <param name="includeReferences">If is true it will load all dlls references of the current project to find all jobs.</param>
        /// <param name="assembliess"></param>
        [PublicAPI]
        public static IGlobalConfiguration UseJobManagement(this IGlobalConfiguration config, bool includeReferences = false, [NotNull] params Assembly[] assemblies) {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            StorageAssemblySingleton.GetInstance().SetCurrentAssembly(includeReferences, assemblies);
            PeriodicJobBuilder.GetAllJobs();
            CreateManagementJob();
            return config;
        }

        [PublicAPI]
        public static IGlobalConfiguration UseJobManagement(this IGlobalConfiguration config) {
            CreateManagementJob();
            return config;
        }

        private static void CreateManagementJob() {
            // pages
            DashboardRoutes.Routes.AddRazorPage(Pages.JobManagement.PageRoute, x => new Pages.JobManagement());
            DashboardRoutes.Routes.AddRazorPage(JobsStoppedPage.PageRoute, x => new JobsStoppedPage());
            DashboardRoutes.Routes.AddRazorPage(SettingsPage.PageRoute, x => new SettingsPage());
            DashboardRoutes.Routes.AddRazorPage(NotificationsPage.PageRoute, x => new NotificationsPage());

            // routes
            DashboardRoutes.Routes.Add("/jobs/GetJobsStopped", new GetJobsStoppedDispatcher());
            DashboardRoutes.Routes.Add("/JobConfiguration/GetJobs", new GetJobDispatcher());
            DashboardRoutes.Routes.Add("/JobConfiguration/UpdateJobs", new ChangeJobDispatcher());
            DashboardRoutes.Routes.Add("/JobConfiguration/GetJob", new GetJobForEdit());
            DashboardRoutes.Routes.Add("/JobConfiguration/JobAgent", new JobAgentDispatcher());
            DashboardRoutes.Routes.Add("/DataConfiguration/GetTimeZones", new GetTimeZonesDispatcher());

            // jobs stopped
            DashboardMetrics.AddMetric(TagDashboardMetrics.JobsStoppedCount);

            // sidebar
            JobsSidebarMenu.Items.Add(page => new MenuItem("Jobs Stopped", page.Url.To("/jobs/stopped")) {
                Active = page.RequestPath.StartsWith("/jobs/stopped"),
                Metric = TagDashboardMetrics.JobsStoppedCount,
            });

            // navbar
            NavigationMenu.Items.Add(page => new MenuItem(Pages.JobManagement.Title, page.Url.To(Pages.JobManagement.PageRoute))
            {
                Active = page.RequestPath.StartsWith(Pages.JobManagement.PageRoute),
                Metric = DashboardMetrics.RecurringJobCount
            });

            NavigationMenu.Items.Add(page => new MenuItem(NotificationsPage.Title, page.Url.To(NotificationsPage.PageRoute))
            {
                Active = page.RequestPath.StartsWith(NotificationsPage.PageRoute)
            });

            NavigationMenu.Items.Add(page => new MenuItem(SettingsPage.Title, page.Url.To(SettingsPage.PageRoute))
            {
                Active = page.RequestPath.StartsWith(SettingsPage.PageRoute)
            });

            // resources
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/css/jobExtension", "text/css", "Hangfire.JobManagement.Dashboard.Content.css.JobExtension.css");
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/css/cron-expression-input", "text/css", "Hangfire.JobManagement.Dashboard.Content.css.cron-expression-input.css");
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/page", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.jobextension.js");

            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/vue", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.vue.js");
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/vue3", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.vue.3.4.27.js");

            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/axio", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.axios.min.js");
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/daysjs", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.daysjs.min.js");
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/relativeTime", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.relativeTime.min.js");
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/vuejsPaginate", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.vuejs-paginate.js");
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/sweetalert", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.sweetalert.js");
            AddDashboardRouteToEmbeddedResource("/JobConfiguration/js/cron-expression-input", "application/javascript", "Hangfire.JobManagement.Dashboard.Content.js.cron-expression-input.js");
        }

        private static void AddDashboardRouteToEmbeddedResource(string route, string contentType, string resourceName)
           => DashboardRoutes.Routes.Add(route, new ContentDispatcher(contentType, resourceName, TimeSpan.FromDays(1)));
    }
}
