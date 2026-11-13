using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// launch profile
var launchProfileName = builder.Configuration["DOTNET_LAUNCH_PROFILE"] ?? builder.Configuration["LAUNCH_PROFILE"] ?? "LOCAL";

// projects
builder.AddProject<Hangfire_JobManagement_Test_SampleServer>("hangfire-jobmanagement", launchProfileName);

builder.Build().Run();
