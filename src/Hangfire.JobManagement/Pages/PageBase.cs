using Hangfire.Dashboard;
using Hangfire.JobManagement.Core;

namespace Hangfire.JobManagement.Pages
{
    internal abstract class PageBase : RazorPage
    {
        public string ResourcesHtml { get; set; }
        public string SidebarHeader { get; set; }

        public override void Execute() { }

        public PageBase()
        {
            ResourcesHtml = Utility.ReadStringResource("Hangfire.JobManagement.Dashboard.Resources.html");
            SidebarHeader = Utility.ReadStringResource("Hangfire.JobManagement.Dashboard.SidebarHeader.html");
        }

        protected void WriteResources()
        {
            WriteLiteral(ResourcesHtml);
            WriteLiteral("\r\n");
        }

        protected void WriteSidebarHeader()
        {
            WriteLiteral(SidebarHeader);
            WriteLiteral("\r\n");
        }

        protected void WriteLiteralLine(string textToAppend) {
            WriteLiteral(textToAppend);
            WriteLiteral("\r\n");
        }

        protected void WriteEmptyLine() {
            WriteLiteral("\r\n");
        }
    }
}
