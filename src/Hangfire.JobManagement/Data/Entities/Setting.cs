﻿using Hangfire.JobManagement.Data.Entities.Interfaces;
using System;

namespace Hangfire.JobManagement.Data.Entities;

internal class Setting : IModelTimeStamps
{
    public string Name { get; set; }

    public string Type { get; set; }

    public dynamic? Value { get; set; }

    public DateTime? CreatedOn { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
