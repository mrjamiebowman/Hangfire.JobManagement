
namespace Hangfire.JobManagement.Data.Entities
{
    public class GlobalSettings
    {
        public long GlobalSettingId { get; set; }

        public string DefaultTimeZoneId { get; set; }

        public string DefaultQueue { get; set; }
    }
}
