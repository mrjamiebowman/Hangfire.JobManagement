using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// projects
builder.AddProject<Hangfire_JobManagement_Test_SampleServer>("test-hangfire");

builder.Build().Run();
