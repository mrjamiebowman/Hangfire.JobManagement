//using Hangfire.JobManagement.Data.Entities;

//namespace Hangfire.JobManagement.Data
//{
//    internal class JobManagementDbContext : DbContext
//    {
//        public JobManagementDbContext()
//        {

//        }

//        public JobManagementDbContext(string connectionString) : base(connectionString)
//        {
//            Database.SetInitializer(new MigrateDatabaseToLatestVersion<JobManagementDbContext, MigrationsConfiguration>());
//        }

//        public DbSet<Batch> Batches { get; set; }

//        public DbSet<JobHistory> JobHistory { get; set; }

//        public DbSet<NotificationGroup> NotificationGroups { get; set; }

//        public DbSet<Notification> Notifications { get; set; }

//        public DbSet<Setting> Settings { get; set; }
//    }
//}
