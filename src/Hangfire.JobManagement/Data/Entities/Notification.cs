using Hangfire.JobManagement.Data.Entities.Interfaces;
using System;

namespace Hangfire.JobManagement.Data.Entities;

internal class Notification : IModelTimeStamps
{

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
