using Hangfire.Dashboard.Pages;
using Hangfire.JobManagement.Core;

namespace Hangfire.JobManagement.Pages;

internal sealed class JobsStoppedPage : PageBase
{
    public const string Title = "Stopped Jobs";
    public const string PageRoute = "/jobs/stopped";

    private static readonly string PageHtml;

    static JobsStoppedPage() 
    {
        PageHtml = Utility.ReadStringResource("Hangfire.JobManagement.Dashboard.JobsStopped.html");
    }

    public override void Execute()
    {
        using var activity = OTel.Application.StartActivity($"{nameof(JobManagement)}.{nameof(Execute)}");

        WriteResources();
        WriteEmptyLine();
        Layout = new LayoutPage(Title);
        WriteSidebarHeader();
        Write(Html.JobsSidebar());
        WriteLiteralLine(PageHtml);
        WriteEmptyLine();
    }
}
