using System.Collections.Generic;
using System.Reflection;

namespace Hangfire.JobManagement.Configuration;

public class JobManagementBuilder
{
    public List<Assembly> Assemblies { get; set; } = new List<Assembly>();

    public JobManagementFeatures Features { get; set; } = new JobManagementFeatures();

    public JobManagementBuilder()
    {
        this.ConfigureDefaultServices();
    }
}
