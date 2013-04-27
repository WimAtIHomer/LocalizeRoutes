using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using IHomer.Services.LocalizeRoutes.Entities;
using System.Data.Entity.Infrastructure;

namespace IHomer.Services.LocalizeRoutes.Context
{
    public class ResourceDB : DbContext
    {
        public DbSet<Language> Languages { get; set; }
        public DbSet<Resource> Resources { get; set; }

        protected override DbEntityValidationResult ValidateEntity(DbEntityEntry entityEntry, IDictionary<object, object> items)
        {
            var myItems = new Dictionary<object, object> { { "Context", this } };

            return base.ValidateEntity(entityEntry, myItems);
        }
    }
}
