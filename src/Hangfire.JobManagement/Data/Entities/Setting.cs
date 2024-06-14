using Hangfire.JobManagement.Data.Entities.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hangfire.JobManagement.Data.Entities;

public class Setting : IModelTimeStamps
{
    public long SettingId { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    [NotMapped]
    public dynamic? Value { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
