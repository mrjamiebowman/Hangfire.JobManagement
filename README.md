# Hangfire.JobManagement
[![NuGet](https://buildstats.info/nuget/Hangfire.RecurringJobAdmin)](https://www.nuget.org/packages/Hangfire.RecurringJobAdmin/)
[![Build status](https://ci.appveyor.com/api/projects/status/u2xrias2vk727beg/branch/master?svg=true)](https://ci.appveyor.com/project/bamotav/hangfire-recurringjobadmin/branch/master)
[![Build Status](https://github.com/bamotav/Hangfire.RecurringJobAdmin/workflows/CI-HRJ/badge.svg)](https://github.com/bamotav/Hangfire.RecurringJobAdmin/actions)
[![Official Site](https://img.shields.io/badge/site-hangfire.io-blue.svg)](http://hangfire.io)
[![License MIT](https://img.shields.io/badge/license-MIT-green.svg)](http://opensource.org/licenses/MIT)

This is a fork that's going a different direction. Just getting started... Below will be updated. - 05/27/2024 @mrjamiebowman


![dashboard](Content/dashboard.png)

A simple dashboard to extend Hangfire's dashboard.

## Instructions
Install a package from Nuget. 
```
Install-Package Hangfire.JobManagement
```

Then add this in your code:

## For .NET  :
for service side:
```csharp
services.AddHangfire(config => config.UseSqlServerStorage(Configuration.GetConnectionString("HangfireConnection"))
                                                 .UseJobManagement(typeof(Startup).Assembly))
```

## For .NET Framework  :
for startup side:
```csharp
GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireConnection").UseJobManagement(typeof(Startup).Assembly)
```

## Credits
 * Braulio Alvarez - original developer of Hangfire.RecurringJobAdmin

## License
Authored by: Brayan Mota (bamotav)

This project is under MIT license. You can obtain the license copy [here](https://github.com/bamotav/Hangfire.RecurringJobAdmin/blob/master/LICENSE).

