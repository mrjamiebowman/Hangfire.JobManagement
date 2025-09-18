# mrjamiebowman.Hangfire.JobManagement (.NET 8)
[![NuGet](https://img.shields.io/nuget/v/MyNugetPackage.svg)](https://github.com/mrjamiebowman/Hangfire.JobManagement/)
[![Build status](https://ci.appveyor.com/api/projects/status/u2xrias2vk727beg/branch/master?svg=true)](https://ci.appveyor.com/project/mrjamiebowman/hangfire-jobmanagement/branch/main)
[![Build Status](https://github.com/mrjamiebowman/Hangfire.JobManagement/actions/workflows/dotnet.yml/badge.svg)](https://github.com/mrjamiebowman/Hangfire.JobManagement/actions)
[![Official Site](https://img.shields.io/badge/site-hangfire.io-blue.svg)](http://hangfire.io)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](http://opensource.org/licenses/MIT)

*NOTE:* This is a fork that's going a different direction. I appologize but I'm having to learn how to publish and manage
        public nuget packages. 09/18/2025 - @mrjamiebowman

A robust extension to .NET Hangfire that adds a Job Management Dashboard, Notifications, and Settings. 

#### Features (Coming Soon!)
* Dashboard
* Start / Stops Jobs
* Edit Jobs
* Job Notifications (Web Hooks, Message Queues, Emails)
* Search Capabilities
* Batch Jobs
* Job History
* Settings

#### Requirements
* MSSQL Server
* .NET 8

#### Dockerized & Ready for Kubernetes (Helm Charts)
// TODO: Share GitHub Sample.

## Instructions
Install a package from NuGet. 
```
Install-Package Hangfire.JobManagement
```

Then add this in your code:

## For .NET
for service side:

```csharp
// job management
builder.Services.ConfigureJobManagement(builder.Configuration);

// hangfire
services.AddHangfire(h => h.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"))
    /* job management */
    .UseJobManagement(jobManagementConfig, b => {
        // assemblies
        b.ConfigureAssemblies(new Assembly[] {
            typeof(Program).Assembly
        });

        // database
        b.ConfigureDatabase();

        // features
        b.ConfigureFeatures(f => {
            f.Notifications = true;
            f.Settings = true;
        });
    })
```

## Controller Job Management
Typically, I use a controller to rapidly set up (Add or Update) or remove all of the jobs rapidly. This is useful for deploying to multiple environments.   

```csharp
app.MapGet("/setup/jobs", () =>
{
    var recurringJobOptions = new RecurringJobOptions();

    // jobs
    RecurringJob.AddOrUpdate<IJobLogCleanUp>("LogCleanup", job => job.ExecuteAsync(null, null, CancellationToken.None), Cron.Daily);
    RecurringJob.AddOrUpdate<IJobReports>("Reports", job => job.ExecuteAsync(null, null, CancellationToken.None), Cron.Daily);

    return "OK";
})
    .WithName("JobSetup")
    //.RequireAuthorization()
    .WithOpenApi();

app.MapGet("/setup/remove", () =>
{
    // remove if exists...
    RecurringJob.RemoveIfExists("LogCleanup");
    RecurringJob.RemoveIfExists("Reports");

    return "OK";
})
    .WithName("JobRemoval")
    //.RequireAuthorization()
    .WithOpenApi();

app.MapGet("/up", () => {
    return "UP";
})
    .WithName("up")
    .WithOpenApi();
```

## EntityFramework Core

```bash
# install tool
dotnet tool install --global dotnet-ef

# migrations
dotnet ef migrations add -v -p ./src/Hangfire.JobManagement/Hangfire.JobManagement.csproj -c JobManagementDbContext -s ../../src/MrJB.Hangfire/ -o ./src/Hangfire.JobManagement/Data/Migrations initial
```

## Credits
This project was inspired by other Hangfire projects and forked from Brayan Mota's RecurringJobAdmin.

 * Braulio Alvarez - original developer of Hangfire.RecurringJobAdmin
 * Brayan Mota (bamotav) - forked his project.

## License
This project is under MIT license. You can obtain the license copy [here](https://github.com/mrjamiebowman/mrjamiebowman.Hangfire.RecurringJobAdmin/blob/master/LICENSE).

