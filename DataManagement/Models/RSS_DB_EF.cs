namespace DataManagement.Models
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class RSS_DB_EF : DbContext
    {
        // Your context has been configured to use a 'RSS_DB_EF' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DataManagement.Models.RSS_DB_EF' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'RSS_DB_EF' 
        // connection string in the application configuration file.
        public RSS_DB_EF()
            : base("name=RSS_DB_EF")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<UserModel> Users { get; set; }
        public virtual DbSet<FileModel> Files { get; set; }
        public virtual DbSet<GroupModel> Groups { get; set; }
        public virtual DbSet<GroupFileSharingModel> GroupFileSharings { get; set; }
        public virtual DbSet<GroupMemberModel> GroupMembers { get; set; }
        public virtual DbSet<ResetPasswordModel> ResetPasswords { get; set; }
        public virtual DbSet<VerifyAccountModel> VerifyAccounts { get; set; }
        public virtual DbSet<UserPlanModel> UserPlans { get; set; }
        public virtual DbSet<PlanModel> Plans { get; set; }
        public virtual DbSet<TokenUsersModel> Tokens { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}