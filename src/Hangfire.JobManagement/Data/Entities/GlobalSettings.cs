using System.ComponentModel.DataAnnotations;

namespace Hangfire.JobManagement.Data.Entities
{
    internal class GlobalSettings
    {
        [Key]
        public long GlobalSettingId { get; set; }

        public string DefaultTimeZoneId { get; set; }

        public string DefaultQueue { get; set; }
    }
}
