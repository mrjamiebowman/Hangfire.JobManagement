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
        System.Diagnostics.Debugger.Break();
    }

    public JobManagementDbFactory(JobManagementConfiguration jobManagementConfiguration)
    {
        System.Diagnostics.Debugger.Break();

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
        // used for debugging... (typically commented out unless needed.)
        //System.Diagnostics.Debugger.Launch();

        // paths
        var parent = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
        var appSettingsPath = Path.Combine(parent, "Enterprise.App.Hangfire");

        var config = new ConfigurationBuilder()
            .SetBasePath(appSettingsPath)
            .AddEnvironmentVariables()
            .AddJsonFile("appsettings.json", false)
            //.AddAzureAppConfiguration(options =>
            //{
            //    var credentials = new DefaultAzureCredential();

            //    // labels
            //    options.Select(KeyFilter.Any, "Hangfire-DEV");

            //    options.Connect(Environment.GetEnvironmentVariable("VAR"))
            //        .ConfigureKeyVault(kv => kv.SetCredential(credentials));
            //})
            .Build();

        // hangfire configuration
        JobManagementConfiguration jobManagementConfiguration = new JobManagementConfiguration();
        config.GetSection(JobManagementConfiguration.Position).Bind(jobManagementConfiguration);

        var builder = new DbContextOptionsBuilder<JobManagementDbContext>();

        return new JobManagementDbContext(builder.Options, jobManagementConfiguration);
    }
}
