using System.Collections.Generic;
using System.Linq;
using System.Web.Compilation;
using System.Globalization;
using System.Resources;
using IHomer.Services.LocalizeRoutes.Repositories;
using IHomer.Services.LocalizeRoutes.Entities;
using System.Web.Mvc;

namespace IHomer.Services.LocalizeRoutes
{
    /// <summary>
    /// Implementation of a very simple route database Resource Provider.
    /// </summary>
    public class RouteResourceProvider : IResourceProvider
    {
        /// <summary>
        /// DefaultLanguage is the language that is used if requested language is not in Languages list
        /// </summary>
        internal readonly static Language DefaultLanguage;

        /// <summary>
        /// List of all languages with resources
        /// </summary>
        internal readonly static Dictionary<string, Language> Languages;

        /// <summary>
        /// Keep track of the 'className' passed by ASP.NET
        /// which is the route in the database.
        /// </summary>
        private readonly string _resourceRoute;

        public string ResourceRoute
        {
            get { return _resourceRoute; }
        } 

        /// <summary>
        /// Cache for each culture of this ResourceSet. Once
        /// loaded we just cache the resources.
        /// </summary>
        private Dictionary<string, Dictionary<string, object>> _resourceCache;

        /// <summary>
        /// Cache for used resourceKeys.
        /// </summary>
        private readonly Dictionary<string, bool> _usedResourceKeys = new Dictionary<string,bool>();

        /// <summary>
        /// Critical section for loading Resource Cache safely
        /// </summary>
        private static readonly object _syncLock = new object();

        /// <summary>
        /// static constructor, retreive static resources Languages and DefaultLanguage
        /// </summary>
        static RouteResourceProvider()
        {
            var repo = new ResourceRepository();
            DefaultLanguage = repo.GetDefaultLanguage();
            Languages = repo.GetLanguages();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="route"></param>
        public RouteResourceProvider(string route)
        {
            _resourceRoute = route;
        }

        /// <summary>
        /// Manages caching of the Resource Sets. Once loaded the values are loaded from the 
        /// cache only.
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        private Dictionary<string, object> GetResourceCache(string language)
        {
            if (_resourceCache == null)
            {
                _resourceCache = new Dictionary<string, Dictionary<string, object>>();
            }

            lock (_syncLock)
            {
                if (_resourceCache.ContainsKey(language))
                {
                    return _resourceCache[language];
                }

                // *** Use ResourceRepository to retrieve the resource keys from the database
                var data = new ResourceRepository();

                var resourceList = data.GetResources(language, _resourceRoute);
                var resources = resourceList.ToDictionary(resource => resource.Key, ConvertDbResource);
                _resourceCache[language] = resources;
                foreach (var resource in resources)
                {
                    if (_usedResourceKeys.ContainsKey(resource.Key))
                    {
                        _usedResourceKeys[resource.Key] = true;
                    }
                    else 
                    {
                        _usedResourceKeys.Add(resource.Key, true);
                    }
                }
                return resources;
            }
        }

        private static object ConvertDbResource(Resource resource)
        {
            if (resource.ResourceType == ResourceType.Html)
            {
                return new MvcHtmlString(resource.Value);
            }
            return resource.Value;
        }


        /// <summary>
        /// The main worker method that retrieves a resource key for a given culture
        /// from a ResourceSet.
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object GetObject(string resourceKey, CultureInfo culture)
        {
            var language = GetLanguageName(culture ?? CultureInfo.CurrentUICulture);
            var resources = GetResourceCache(language);

            lock (_syncLock)
            {
                if (!resources.ContainsKey(resourceKey))
                {
                    var resource = FindMissingResource(resourceKey, language);
                    resources.Add(resourceKey, resource == null ? Resource.SplitResourceCode(resourceKey) : ConvertDbResource(resource));
                    if (!_usedResourceKeys.ContainsKey(resourceKey))
                    {
                        _usedResourceKeys.Add(resourceKey, resource == null);
                    }
                    else
                    {
                        _usedResourceKeys[resourceKey] = (resource != null);
                    }
                }
            }
            return resources[resourceKey];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceKey"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public Resource FindMissingResource(string resourceKey, string language)
        {
            var repo = new ResourceRepository();
            var route = _resourceRoute;
            var resource = repo.GetResource(resourceKey, route, language);
            while (route != Resource.ROUTE_SEPARATOR && resource == null)
            {
                route = Resource.GetUpperRoute(route);
                resource = repo.GetResource(resourceKey, route, language);
            }
            if (resource == null && language != DefaultLanguage.Name)
            {
                return FindMissingResource(resourceKey, DefaultLanguage.Name);
            }
            return resource;
        }

        /// <summary>
        /// The Resource Reader is used parse over the resource collection
        /// that the ResourceSet contains. It's basically an IEnumarable interface
        /// implementation and it's what's used to retrieve the actual keys
        /// </summary>
        public IResourceReader ResourceReader  // IResourceProvider.ResourceReader
        {
            get
            {
                return new RouteResourceReader(_usedResourceKeys);
            }
        }

        /// <summary>
        /// clears the cache, after a update from a resource for example
        /// </summary>
        public void ClearCache()
        {
            if (_resourceCache != null)
            {
                _resourceCache.Clear();
            }
        }

        /// <summary>
        /// clears the cache for a specific language, after a update from a resource for example
        /// </summary>
        public void ClearCache(string language)
        {
            if (_resourceCache != null && _resourceCache[language] != null)
            {
                _resourceCache.Remove(language);
            }
        }

        /// <summary>
        /// removes a key from the usedResourceKeys list
        /// </summary>
        /// <param name="key"></param>
        public void RemoveKey(string key)
        {
            if (_usedResourceKeys.ContainsKey(key))
                _usedResourceKeys.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static string GetLanguageName(CultureInfo culture)
        {
            return culture == null
                       ? DefaultLanguage.Name
                       : (Languages.ContainsKey(culture.Name)
                              ? culture.Name
                              : (Languages.ContainsKey(culture.TwoLetterISOLanguageName)
                                     ? culture.TwoLetterISOLanguageName
                                     : DefaultLanguage.Name));
        }
    }
}
