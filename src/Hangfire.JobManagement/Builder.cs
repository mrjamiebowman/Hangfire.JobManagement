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

            // routes sidebar
            DashboardRoutes.Routes.Add("/jobs/GetJobsStopped", new GetJobsStoppedDispatcher());

            // routes
            DashboardRoutes.Routes.Add("/management/data/GetJobs", new GetJobDispatcher());
            DashboardRoutes.Routes.Add("/management/data/UpdateJobs", new ChangeJobDispatcher());
            DashboardRoutes.Routes.Add("/management/data/GetJob", new GetJobForEdit());
            DashboardRoutes.Routes.Add("/management/data/JobAgent", new JobAgentDispatcher());
            DashboardRoutes.Routes.Add("/management/data/timezones", new GetTimeZonesDispatcher());

            // jobs stopped
            DashboardMetrics.AddMetric(TagDashboardMetrics.JobsStoppedCount);

            // sidebar
            JobsSidebarMenu.Items.Add(page => new MenuItem("Jobs Stopped", page.Url.To(JobsStoppedPage.PageRoute)) {
                Active = page.RequestPath.StartsWith(JobsStoppedPage.PageRoute),
                Metric = TagDashboardMetrics.JobsStoppedCount,
            });

            // navbar
            NavigationMenu.Items.Add(page => new MenuItem(Pages.JobManagement.Title, page.Url.To(Pages.JobManagement.PageRoute))
            {
                Active = page.RequestPath.StartsWith(Pages.JobManagement.PageRoute),
                Metric = DashboardMetrics.RecurringJobCount
            });

            // coming soon!
            //NavigationMenu.Items.Add(page => new MenuItem(NotificationsPage.Title, page.Url.To(NotificationsPage.PageRoute))
            //{
            //    Active = page.RequestPath.StartsWith(NotificationsPage.PageRoute)
            //});

            // coming soon!
            NavigationMenu.Items.Add(page => new MenuItem(SettingsPage.Title, page.Url.To(SettingsPage.PageRoute))
            {
                Active = page.RequestPath.StartsWith(SettingsPage.PageRoute)
            });

            // resources
            AddDashboardRouteToEmbeddedResource("/resources/css/jobExtension", "text/css", "Hangfire.JobManagement.Dashboard.Content.css.JobExtension.css");
            AddDashboardRouteToEmbeddedResource("/resources/css/cron-expression-input", "text/css", "Hangfire.JobManagement.Dashboard.Content.css.cron-expression-input.css");
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
