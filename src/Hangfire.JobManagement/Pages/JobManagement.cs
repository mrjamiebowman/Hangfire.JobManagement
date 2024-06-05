using Hangfire.Dashboard.Pages;
using Hangfire.JobManagement.Core;

namespace Hangfire.JobManagement.Pages
{
    internal sealed class JobManagement : PageBase
    {
        public const string Title = "Job Management";
        public const string PageRoute = "/management/configuration";

        private static readonly string PageHtml;

        static JobManagement() {
            PageHtml = Utility.ReadStringResource("Hangfire.JobManagement.Dashboard.JobManagement.html");
        }

        public override void Execute() {
            WriteEmptyLine();
            Layout = new LayoutPage(Title);
            WriteLiteralLine(PageHtml);
            WriteEmptyLine();
        }
    }
}
