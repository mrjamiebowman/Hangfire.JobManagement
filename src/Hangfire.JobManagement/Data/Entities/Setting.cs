namespace Hangfire.JobManagement.Data.Entities;

internal class Setting
{
    public string Name { get; set; }

    public string Type { get; set; }

    public dynamic? Value { get; set; }
}
