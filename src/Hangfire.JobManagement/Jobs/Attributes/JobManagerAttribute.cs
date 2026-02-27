using Hangfire.States;
using System;

namespace Hangfire.JobManagement.Jobs.Attributes;

/// <summary>
/// Attribute to add or update <see cref="RecurringJob"/> automatically
/// by target it to interface/instance/static method.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public class JobManagerAttribute : Attribute
{
    public string? Name { get; set; }

    public JobManagerAttribute(string name)
    {
        Name = name;
    }
}
