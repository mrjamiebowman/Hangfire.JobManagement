using Hangfire.JobManagement.Configuration;
using Hangfire.JobManagement.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hangfire.JobManagement.Data
{
    public class JobManagementDbContext : DbContext
    {

        // configuration
        private readonly JobManagementConfiguration _jobManagementConfiguration;

        public JobManagementDbContext()
        {

        }

        public JobManagementDbContext(DbContextOptions<JobManagementDbContext> options, JobManagementConfiguration jobManagementConfiguration) : base(options)
        {
            _jobManagementConfiguration = jobManagementConfiguration;
        }

        public DbSet<Batch> Batches { get; set; }

        public DbSet<JobHistory> JobHistory { get; set; }

        public DbSet<NotificationGroup> NotificationGroups { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<Setting> Settings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_jobManagementConfiguration.ConnectionString);
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.EnableDetailedErrors();
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Entity>(b => {
            //    b.HasKey(e => e.EntityId);
            //    b.Property(e => e.EntityId).ValueGeneratedOnAdd();
            //});
        }

        protected virtual void SeedDatabase()
        {
            // ensure db is created
            this.Database.EnsureCreated();

            // verify test data
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            var changedEntities = ChangeTracker
                .Entries()
                .Where(_ => _.State == EntityState.Added ||
                            _.State == EntityState.Modified);

            var errors = new List<ValidationResult>(); // all errors are here

            foreach (var e in changedEntities)
            {
                var vc = new ValidationContext(e.Entity, null, null);
                Validator.TryValidateObject(e.Entity, vc, errors, validateAllProperties: true);
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
