using Hangfire.JobManagement.Data.Entities.Interfaces;
using System;

namespace Hangfire.JobManagement.Data.Entities;

public class Notification : IModelTimeStamps
{
    public long NotificationId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
