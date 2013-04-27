using IHomer.Services.LocalizeRoutes;

namespace WebsiteLocalizeRoutesService.Resources
{
    public class Resource
    {
        private static readonly dynamic _resource = new RouteResourceStrings();

        public static object Message
        {
            get
            {
                return RouteResourceService.GetResource("Message");
            }
        }

        public static object EditResourceExplanation
        {
            get { return _resource.EditResourceExplanation; }
        }

        public static object Save
        {
            get { return _resource.Save; }
        }

        public static object Add
        {
            get { return _resource.Add; }
        }

        public static object Delete
        {
            get { return _resource.Delete; }
        }

        public static object RouteAndLanguage
        {
            get { return _resource.RouteAndLanguage; }
        }

        public static object Route
        {
            get { return _resource.Route; }
        }

        public static object Language
        {
            get { return _resource.Language; }
        }

        public static object Get
        {
            get { return _resource.Get; }
        }

        public static object Resources
        {
            get { return _resource.Resources; }
        }

        public static object EditResource
        {
            get { return _resource.EditResource; }
        }
    }
}