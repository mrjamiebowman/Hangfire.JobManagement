using Hangfire;
using Hangfire.JobManagement.Test.SampleServer;
using Hangfire.JobManagement.Test.SampleServer.Configuration;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// hangfire configuration
var hangfireConfiguration = new HangfireConfiguration();
builder.Configuration.GetSection(HangfireConfiguration.Position).Bind(hangfireConfiguration);
builder.Services.AddSingleton<HangfireConfiguration>(hangfireConfiguration);

// hangfire (has to go last bc the HangfireJobActivator takes builder.Services...)
builder.Services.AddHangfire(config =>
{
    config
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseColouredConsoleLogProvider()
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseActivator(new HangfireJobActivator(builder.Services))
            .UseSqlServerStorage(hangfireConfiguration.ConnectionString);
});

// Add the processing server as IHostedService
builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = 3;
    options.Queues = new[] { "default", "status-reports", "status-reports-test" };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    // endpoints.MapRazorPages();
    // endpoints.MapHub<ChatHub>("/hubs/chat");
});

// dashboard options
DashboardOptions dashboardOptions = new DashboardOptions();

// hangfire
app.UseHangfireDashboard(hangfireConfiguration.DashboardUrl, dashboardOptions);

//app.MapGet("/hangfire/reset-counters", async (IHangfireDatabaseRepository repository, HttpContext context) =>
//{
//    await repository.ResetCountersAsync();
//    await context.Response.WriteAsync("Counters reset successfully!");
//});

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

