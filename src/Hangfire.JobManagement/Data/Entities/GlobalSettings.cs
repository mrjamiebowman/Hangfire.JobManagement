
namespace Hangfire.JobManagement.Data.Entities
{
    internal class GlobalSettings
    {
        public long GlobalSettingId { get; set; }

        public string DefaultTimeZoneId { get; set; }

        public string DefaultQueue { get; set; }
    }
}
