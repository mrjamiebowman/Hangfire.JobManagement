using Hangfire.JobManagement.Data.Entities.Interfaces;
using System;

namespace Hangfire.JobManagement.Data.Entities;

public class SettingQueue : IModelTimeStamps
{
    public int? SettingQueueId { get; set; }

    public string QueueName { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
