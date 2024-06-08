using System.ComponentModel.DataAnnotations;

namespace Hangfire.JobManagement.Data.Entities;

internal class NotificationGroup
{
    [Key]
    public long NotificationGroupId { get; set; }

}
