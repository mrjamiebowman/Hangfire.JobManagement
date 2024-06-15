using Hangfire.JobManagement.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Hangfire.JobManagement.Models;

public class GlobalSetting
{
    public string? DefaultTimeZoneId { get; set; }

    public string? DefaultQueue { get; set; }

    public GlobalSetting()
    {

    }

    public GlobalSetting(List<Setting> settings)
    {
        // map
        DefaultTimeZoneId = (settings.SingleOrDefault(x => x.Name == nameof(DefaultTimeZoneId).ToString()))?.Value;
        DefaultQueue = (settings.SingleOrDefault(x => x.Name == nameof(DefaultQueue).ToString()))?.Value;
    }
}
