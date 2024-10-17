using Hangfire.JobManagement.Data.Entities.Interfaces;
using System;
using System.Collections.Generic;

namespace Hangfire.JobManagement.Data.Entities;

public class Batch : IModelTimeStamps
{
    public long BatchId { get; set; }

    public List<BatchOperation> BatchOperations { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
