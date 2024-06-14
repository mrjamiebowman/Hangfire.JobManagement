using Hangfire.JobManagement.Data.Entities.Interfaces;
using System;

namespace Hangfire.JobManagement.Data.Entities
{
    public class BatchOperation : IModelTimeStamps
    {
        public long BatchOperationId { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
