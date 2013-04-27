using System;
using IHomer.Services.LocalizeRoutes;

namespace WebsiteLocalizeRoutesService.Resources
{
    public static class Master
    {
        private static readonly dynamic _resource = new RouteResourceStrings();
        public static Object Title
        {
            get
            {
                return RouteResourceService.GetResource("Title");
            }
        }

        public static Object HomePage
        {
            get
            {
                return RouteResourceService.GetResource("HomePage");
            }
        }

        public static Object AboutPage
        {
            get
            {
                return RouteResourceService.GetResource("AboutPage");
            }
        }

        public static Object EditResources
        {
            get
            {
                return RouteResourceService.GetResource("EditResources");
            }
        }

        public static Object Save
        {
            get { return _resource.Save; }
        }

        public static Object Add
        {
            get
            {
                return RouteResourceService.GetResource("Add");
            }
        }

        public static Object ErrorText
        {
            get
            {
                return RouteResourceService.GetResource("ErrorText");
            }
        }
    }
}