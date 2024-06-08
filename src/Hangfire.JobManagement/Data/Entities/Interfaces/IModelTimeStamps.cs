using System;

namespace Hangfire.JobManagement.Data.Entities.Interfaces
{
    internal interface IModelTimeStamps
    {
        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
