# mrjamiebowman.Hangfire.JobManagement (.NET 8)
[![NuGet](https://buildstats.info/nuget/mrjamiebowman.Hangfire.JobManagement)](https://buildstats.info/nuget/mrjamiebowman.Hangfire.JobManagement)
[![Build status](https://ci.appveyor.com/api/projects/status/u2xrias2vk727beg/branch/master?svg=true)](https://ci.appveyor.com/project/mrjamiebowman/hangfire-jobmanagement/branch/main)
[![Build Status](https://github.com/mrjamiebowman/Hangfire.JobManagement/actions/workflows/dotnet.yml/badge.svg)](https://github.com/mrjamiebowman/Hangfire.JobManagement/actions)
[![Official Site](https://img.shields.io/badge/site-hangfire.io-blue.svg)](http://hangfire.io)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](http://opensource.org/licenses/MIT)

Note: This is a fork that's going a different direction. Just getting started... Below will be updated. - 05/27/2024 @mrjamiebowman

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

## EntityFramework Core

``dotnet ef migrations add -p .\exts\Hangfire.JobManagement\ -v -s .\src\Hangfire.JobManagement\ -o .\src\Hangfire.JobManagement\Data\Migrations initial``

## Credits
This project was inspired by other Hangfire projects and forked from Brayan Mota's RecurringJobAdmin.

 * Braulio Alvarez - original developer of Hangfire.RecurringJobAdmin
 * Brayan Mota (bamotav) - forked his project.

## License
This project is under MIT license. You can obtain the license copy [here](https://github.com/mrjamiebowman/mrjamiebowman.Hangfire.RecurringJobAdmin/blob/master/LICENSE).

