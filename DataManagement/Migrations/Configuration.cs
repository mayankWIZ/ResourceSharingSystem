namespace DataManagement.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DataManagement.Models.RSS_DB_EF>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DataManagement.Models.RSS_DB_EF context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            context.Users.AddOrUpdate(x => x.username ,new Models.UserModel() { username="MMR", usertype="user", dob=DateTime.Now, email="maa@gmail.com", fname="Mayank", lname="Ranghadiya", isActive=true, isVerified=true, password="mmmrrr", lastloggedin=DateTime.Now, lastPassChange= DateTime.Now, profilePic="NotAssigned" });

        }
    }
}
