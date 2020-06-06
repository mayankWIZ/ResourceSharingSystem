namespace DataManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FileModels", "username", "dbo.UserModels");
            DropIndex("dbo.FileModels", new[] { "username" });
            CreateTable(
                "dbo.TokenUsersModels",
                c => new
                    {
                        token = c.String(nullable: false, maxLength: 128),
                        name = c.String(),
                    })
                .PrimaryKey(t => t.token);
            
            AddColumn("dbo.FileModels", "size", c => c.Double(nullable: false));
            AlterColumn("dbo.FileModels", "username", c => c.String(maxLength: 128));
            CreateIndex("dbo.FileModels", "username");
            AddForeignKey("dbo.FileModels", "username", "dbo.UserModels", "username");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FileModels", "username", "dbo.UserModels");
            DropIndex("dbo.FileModels", new[] { "username" });
            AlterColumn("dbo.FileModels", "username", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.FileModels", "size");
            DropTable("dbo.TokenUsersModels");
            CreateIndex("dbo.FileModels", "username");
            AddForeignKey("dbo.FileModels", "username", "dbo.UserModels", "username", cascadeDelete: true);
        }
    }
}
