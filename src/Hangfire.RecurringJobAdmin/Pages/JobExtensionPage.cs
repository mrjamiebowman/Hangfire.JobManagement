using Hangfire.Dashboard;
using Hangfire.Dashboard.Pages;
using Hangfire.JobManagement.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfire.JobManagement.Pages
{
    internal sealed class JobExtensionPage : PageBase
    {
        public const string Title = "Job Configuration";
        public const string PageRoute = "/JobConfiguration";

        private static readonly string PageHtml;

        static JobExtensionPage() {
            PageHtml = Utility.ReadStringResource("Hangfire.RecurringJobAdmin.Dashboard.JobExtension.html");
        }

        public override void Execute() {
            WriteEmptyLine();
            Layout = new LayoutPage(Title);
            WriteLiteralLine(PageHtml);
            WriteEmptyLine();
        }
    }
}
