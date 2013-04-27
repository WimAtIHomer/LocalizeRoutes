using System.Data.Entity;

namespace IHomer.Services.LocalizeRoutes.Context
{
    public class ResourceDBInitializer : CreateDatabaseIfNotExists<ResourceDB>
    {
        protected override void Seed(ResourceDB context)
        {
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX IX_Resources ON Resources (LanguageID, Route, Key)");
        }
    }
}
