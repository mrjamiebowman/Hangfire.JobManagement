using Hangfire;
using Hangfire.Console;
using Hangfire.JobManagement;
using Hangfire.JobManagement.Test.Jobs.Configuration;
using Hangfire.JobManagement.Test.SampleServer;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

// service defaults
builder.AddServiceDefaults();

/************************************************/
/*                  logging                     */
/************************************************/

// serilog
var loggerConfiguration = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .WriteTo.Console()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("Service Name", "Job Manager")
        .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code);

Log.Logger = loggerConfiguration.CreateLogger();

// starting identity server
Log.Information("Starting Job Manager (Hangfire)...");

// serilog
builder.Logging
    .ClearProviders()
    .AddSerilog(Log.Logger);

/************************************************/
/*                  app                         */
/************************************************/

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/************************************************/
/*                  hangfire                    */
/************************************************/

// hangfire configuration
var hangfireConfiguration = new HangfireConfiguration();
builder.Configuration.GetSection(HangfireConfiguration.Position).Bind(hangfireConfiguration);
builder.Services.AddSingleton<HangfireConfiguration>(hangfireConfiguration);

// JobManagement
builder.ConfigureJobManagement();

// hangfire (has to go last bc the HangfireJobActivator takes builder.Services...)
builder.Services.AddHangfire(config =>
{
    config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
        .UseColouredConsoleLogProvider()
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        //.UseActivator(new HangfireJobActivator(builder.Services))
        .UseSqlServerStorage(hangfireConfiguration.ConnectionString)

        // Job Management
        .UseJobManagement(builder, o =>
        {
            o.Assemblies.Add(typeof(Program).Assembly);
        })

        // Console - https://github.com/pieceofsummer/Hangfire.Console
        .UseConsole()
    ;
});

// Add the processing server as IHostedService
builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = 3;
    options.Queues = new[] { "default", "alt-queue" };
});

// jobs
builder.ConfigureJobs();

/************************************************/
/*                  build                       */
/************************************************/

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseAuthorization();
app.MapControllers();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// dashboard options
DashboardOptions dashboardOptions = new DashboardOptions();

// hangfire
app.UseHangfireDashboard(hangfireConfiguration.DashboardUrl, dashboardOptions);

app.MapDefaultEndpoints();

try
{
    await app.RunAsync();
} catch (Exception ex)
{
    Log.Logger.Fatal(ex, "FATAL ERROR: {error}", ex.Message);
}
finally
{
    await Log.CloseAndFlushAsync();
}

