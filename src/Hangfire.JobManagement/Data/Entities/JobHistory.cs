using Hangfire.JobManagement.Data.Entities.Interfaces;
using System;

namespace Hangfire.JobManagement.Data.Entities
{
    internal class JobHistory : IModelTimeStamps
    {
        public long JobHistoryId { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
