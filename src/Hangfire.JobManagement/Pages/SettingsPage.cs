using Hangfire.Dashboard.Pages;
using Hangfire.JobManagement.Core;

namespace Hangfire.JobManagement.Pages
{
    internal class SettingsPage : PageBase
    {
        public const string Title = "Settings";
        public const string PageRoute = "/management/settings";

        private static readonly string PageHtml;

        static SettingsPage()
        {
            PageHtml = Utility.ReadStringResource("Hangfire.JobManagement.Dashboard.Settings.html");
        }

        public override void Execute()
        {
            WriteEmptyLine();
            Layout = new LayoutPage(Title);
            WriteResources();
            WriteLiteralLine(PageHtml);
            WriteEmptyLine();
        }
    }
}
