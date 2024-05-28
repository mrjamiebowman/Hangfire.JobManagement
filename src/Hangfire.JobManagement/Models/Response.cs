namespace Hangfire.JobManagement.Models
{
    public class Response
    {
        public bool Status { get; set; }

        public object Object { get; set; }

        public string Message { get; set; }
    }
}
