using System.Web;
using IHomer.Services.LocalizeRoutes.Repositories;
using IHomer.Services.LocalizeRoutes.Entities;

namespace IHomer.Services.LocalizeRoutes
{
    public class RouteResourceService
    {
        /// <summary>
        /// Op dit moment wordt er gezocht naar resources in het path {area}/{controller}/{action}/{id} waarbij area en id optioneel
        /// Er wordt gezocht naar de eerste resource die gevonden wordt waarbij er steeds een kleiner deel van het path wordt gebruikt
        /// Op het applicatie niveau wordt op / gezocht. Dit zijn dus je global resources. 
        /// </summary>
        /// <param name="code">Naam van de resource</param>
        /// <returns></returns>
        public static object GetResource(string code)
        {
            var routedata = HttpContext.Current.Request.RequestContext.RouteData;
            var route = string.Join(Resource.ROUTE_SEPARATOR, routedata.Values.Values);
            if (routedata.DataTokens.ContainsKey("area") && !routedata.Values.ContainsKey("area") && !string.IsNullOrWhiteSpace(routedata.DataTokens["area"].ToString()))
            {
                route = string.Concat(routedata.DataTokens["area"], Resource.ROUTE_SEPARATOR, route);
            }
            return HttpContext.GetGlobalResourceObject(route, code);
        }

        /// <summary>
        /// Updates the resource in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="languageID"></param>
        /// <param name="route"></param>
        /// <param name="type"></param>
        public static void UpdateResourceString(int id, string key, string value, int languageID, string route, ResourceType type)
        {
            var repo = new ResourceRepository();

            var resource = repo.GetResourceByID(id);
            if (resource.Key != key)
            {
                RouteResourceProviderFactory.RemoveKey(route, key);
            }
            if (resource.LanguageID == languageID)
            {
                repo.UpdateResource(id, key, value, languageID, route, type);
            }
            else 
            {
                var lanResource = repo.GetResource(key, route, languageID);
                if (lanResource != null)
                {
                    repo.UpdateResource(lanResource.ID, key, value, languageID, route, type);
                }
                else
                {
                    repo.AddResource(key, value, languageID, route, type);
                }
            } 
        }

        /// <summary>
        /// Add the resource to the database
        /// </summary>
        /// <param name="code"></param>
        /// <param name="value"></param>
        /// <param name="language"></param>
        /// <param name="route"></param>
        /// <param name="type"></param>
        public static void AddResourceString(string code, string value, int language, string route, ResourceType type)
        {
            var repo = new ResourceRepository();
            repo.AddResource(code, value, language, route, type);
        }

        /// <summary>
        /// Deletes the resource from the database
        /// </summary>
        /// <param name="id"></param>
        public static void DeleteResourceString(int id)
        {
            var repo = new ResourceRepository();
            var resource = repo.GetResourceByID(id);
            RouteResourceProviderFactory.RemoveKey(resource.Route, resource.Key);
            repo.DeleteResource(id);
        }
    }
}
