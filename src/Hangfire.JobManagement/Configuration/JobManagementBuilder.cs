using System.Reflection;

namespace Hangfire.JobManagement.Configuration;

public class JobManagementBuilder
{
    public Assembly[] Assemblies { get; set; }

    public JobManagementFeatures Features { get; set; } = new JobManagementFeatures();

    public JobManagementBuilder()
    {
        // configure default services
        this.ConfigureDefaultServices();
    }
}
