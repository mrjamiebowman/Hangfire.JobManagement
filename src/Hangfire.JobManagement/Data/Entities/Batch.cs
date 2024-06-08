using Hangfire.JobManagement.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hangfire.JobManagement.Data.Entities
{
    internal class Batch : IModelTimeStamps
    {
        [Key]
        public long BatchId { get; set; }

        public List<BatchOperation> BatchOperations { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
