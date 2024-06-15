using Hangfire.JobManagement.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Hangfire.JobManagement.Data;

public class JobManagementDbFactory : IDesignTimeDbContextFactory<JobManagementDbContext>
{
    private readonly JobManagementConfiguration _jobManagementConfiguration;

    public JobManagementDbFactory()
    {

    }

    public JobManagementDbFactory(JobManagementConfiguration jobManagementConfiguration)
    {
        _jobManagementConfiguration = jobManagementConfiguration;
    }

    public JobManagementDbContext Create()
    {
        var options = new DbContextOptionsBuilder<JobManagementDbContext>()
            .UseSqlServer(_jobManagementConfiguration.ConnectionString)
            .Options;

        return new JobManagementDbContext(options, _jobManagementConfiguration);
    }

    /// <summary>
    ///  This is used with the dotnet ef commands
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public JobManagementDbContext CreateDbContext(string[] args)
    {
        System.Diagnostics.Debugger.Launch();

        // paths
        var parent = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        var appPath = Path.Combine(parent, "Hangfire.JobManagement");

        var config = new ConfigurationBuilder()
            .SetBasePath(appPath)
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", true)
            .Build();

        // hangfire configuration
        JobManagementConfiguration jobManagementConfiguration = new JobManagementConfiguration();
        config.GetSection(JobManagementConfiguration.Position).Bind(jobManagementConfiguration);

        var builder = new DbContextOptionsBuilder<JobManagementDbContext>();

        return new JobManagementDbContext(builder.Options, jobManagementConfiguration);
    }
}
