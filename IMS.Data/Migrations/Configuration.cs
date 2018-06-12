namespace IMS.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using IMS.Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<IMS.Data.DataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "IMS.Data.DataContext";
        }

        protected override void Seed(IMS.Data.DataContext context)
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
            context.OfferStatuses.AddOrUpdate(
                s => s.Name,
                new OfferStatus { Name = "New" },
                new OfferStatus { Name = "Accepted" },
                new OfferStatus { Name = "Declined" }
            );
        }
    }
}
