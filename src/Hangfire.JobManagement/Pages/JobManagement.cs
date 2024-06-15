using Hangfire.Dashboard.Pages;
using Hangfire.JobManagement.Core;

namespace Hangfire.JobManagement.Pages
{
    internal sealed class JobManagement : PageBase
    {
        public const string Title = "Job Management";
        public const string PageRoute = "/management/jobs";

        private static readonly string PageHtml;

        static JobManagement() 
        {
            PageHtml = Utility.ReadStringResource("Hangfire.JobManagement.Dashboard.JobManagement.html");
        }

        public override void Execute() 
        {
            using var activity = OTel.Application.StartActivity("JobManagement.Execute");

            WriteEmptyLine();
            Layout = new LayoutPage(Title);
            WriteResources();
            WriteLiteralLine(PageHtml);
            WriteEmptyLine();
        }
    }
}
