using Hangfire.JobManagement.Data.Entities.Interfaces;
using System;

namespace Hangfire.JobManagement.Data.Entities;

public class Setting : IModelTimeStamps
{
    public long SettingId { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public string Value { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public Setting()
    {

    }

    public Setting(string name)
    {
        Name = name;
    }
}
