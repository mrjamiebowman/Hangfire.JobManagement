using Hangfire.JobManagement.Configuration;

namespace Hangfire.JobManagement.Data.Repositories
{
    internal abstract class BaseEfRepository
    {
        // configuration
        protected readonly JobManagementConfiguration _jobManagementConfiguration;

        internal BaseEfRepository(JobManagementConfiguration jobManagementConfiguration)
        {
            _jobManagementConfiguration = jobManagementConfiguration;
        }
    }
}

//string connectionString = "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";

//using (var context = new MyDbContext(connectionString))
//{
//    var entity = new MyEntity { Name = "Test" };
//    context.MyEntities.Add(entity);
//    context.SaveChanges();

//    var entities = context.MyEntities.ToList();
//    foreach (var e in entities)
//    {
//        Console.WriteLine($"ID: {e.Id}, Name: {e.Name}");
//    }
//}
