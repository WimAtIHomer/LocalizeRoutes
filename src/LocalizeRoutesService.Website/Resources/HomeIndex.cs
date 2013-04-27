using System;
using IHomer.Services.LocalizeRoutes;

namespace LocalizeRoutesService.Website.Resources
{
    public class HomeIndex
    {
        public static Object Message
        {
            get
            {
                return RouteResourceService.GetResource("Message");
            }
        }
    }
}