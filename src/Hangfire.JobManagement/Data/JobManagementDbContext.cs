﻿using Hangfire.JobManagement.Data.Entities;
using System.Data.Entity;

namespace Hangfire.JobManagement.Data
{
    internal class JobManagementDbContext : DbContext
    {
        public JobManagementDbContext(string connectionString) : base(connectionString)
        {

        }

        public DbSet<Batch> Batches { get; set; }

        public DbSet<JobHistory> JobHistory { get; set; }

        public DbSet<NotificationGroup> NotificationGroups { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Setting> Settings { get; set; }
    }
}
