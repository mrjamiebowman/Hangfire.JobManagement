namespace Hangfire.JobManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Batches",
                c => new
                    {
                        BatchId = c.Long(nullable: false, identity: true),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.BatchId);
            
            CreateTable(
                "dbo.BatchOperations",
                c => new
                    {
                        BatchOperationId = c.Long(nullable: false, identity: true),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                        Batch_BatchId = c.Long(),
                    })
                .PrimaryKey(t => t.BatchOperationId)
                .ForeignKey("dbo.Batches", t => t.Batch_BatchId)
                .Index(t => t.Batch_BatchId);
            
            CreateTable(
                "dbo.JobHistories",
                c => new
                    {
                        JobHistoryId = c.Long(nullable: false, identity: true),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.JobHistoryId);
            
            CreateTable(
                "dbo.NotificationGroups",
                c => new
                    {
                        NotificationGroupId = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.NotificationGroupId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        NotificationId = c.Long(nullable: false, identity: true),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.NotificationId);
            
            CreateTable(
                "dbo.Settings",
                c => new
                    {
                        SettingId = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Type = c.String(),
                        CreatedOn = c.DateTime(),
                        ModifiedOn = c.DateTime(),
                    })
                .PrimaryKey(t => t.SettingId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BatchOperations", "Batch_BatchId", "dbo.Batches");
            DropIndex("dbo.BatchOperations", new[] { "Batch_BatchId" });
            DropTable("dbo.Settings");
            DropTable("dbo.Notifications");
            DropTable("dbo.NotificationGroups");
            DropTable("dbo.JobHistories");
            DropTable("dbo.BatchOperations");
            DropTable("dbo.Batches");
        }
    }
}
