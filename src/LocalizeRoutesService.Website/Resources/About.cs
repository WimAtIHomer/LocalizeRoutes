using IHomer.Services.LocalizeRoutes;

namespace LocalizeRoutesService.Website.Resources
{
    public static class About
    {
        public static object WimPool
        {
            get
            {
                return RouteResourceService.GetResource("Wim Pool");
            }
        }

        public static object IHomer
        {
            get
            {
                return RouteResourceService.GetResource("IHomer");
            }
        }
    }
}