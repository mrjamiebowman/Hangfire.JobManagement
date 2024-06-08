using Hangfire.JobManagement.Data.Entities.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Hangfire.JobManagement.Data.Entities
{
    internal class BatchOperation : IModelTimeStamps
    {
        [Key]
        public long BatchOperationId { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
