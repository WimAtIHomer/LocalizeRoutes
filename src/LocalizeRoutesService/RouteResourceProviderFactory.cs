using System;
using System.Web;
using System.Collections.Generic;
using System.Web.Compilation;
using IHomer.Services.LocalizeRoutes.Repositories;
using IHomer.Services.LocalizeRoutes.Entities;

namespace IHomer.Services.LocalizeRoutes
{
    /// <summary>
    /// Provider factory that instantiates the individual provider. The provider
    /// passes a 'classname' which is the ResourceSet id or how a resource is identified.
    /// For global resources it's the name of hte resource file, for local resources
    /// it's the full Web relative virtual path
    /// </summary>
    ///[DesignTimeResourceProviderFactoryAttribute(typeof(CustomDesignTimeResourceProviderFactory))]
    public class RouteResourceProviderFactory : ResourceProviderFactory
    {

        private static readonly IDictionary<String, RouteResourceProvider> _resourceCache = new Dictionary<String, RouteResourceProvider>();
        private static readonly List<string> _routes; 

        static RouteResourceProviderFactory()
        {
            var repo = new ResourceRepository();
            _routes = repo.GetRoutes().ConvertAll(r => r.ToLowerInvariant());
        }

        /// <summary>
        /// ASP.NET sets up provides the global resource name which is the 
        /// resource ResX file (without any extensions). This will become
        /// our ResourceSet id. ie. Resource.resx becomes "Resources"
        /// </summary>
        /// <param name="classname"></param>
        /// <returns></returns>
        public override IResourceProvider CreateGlobalResourceProvider(string classname)
        {
            var route = classname.ToLowerInvariant();
            while (!_routes.Contains(route))
            {
                route = Resource.GetUpperRoute(route);
                if (route == Resource.ROUTE_SEPARATOR) break;
            }

            if (!_resourceCache.ContainsKey(route))
                _resourceCache.Add(route, new RouteResourceProvider(route));

            return _resourceCache[route];
        }

        /// <summary>
        /// ASP.NET passes the full page virtual path (/MyApp/subdir/test.aspx) wich is
        /// the effective ResourceSet id. We'll store only an application relative path
        /// (subdir/test.aspx) by stripping off the base path.
        /// </summary>
        /// <param name="virtualPath"></param>
        /// <returns></returns>
        public override IResourceProvider CreateLocalResourceProvider(string virtualPath)
        {
            virtualPath = VirtualPathUtility.ToAppRelative(virtualPath);
            return CreateGlobalResourceProvider(virtualPath);
        }

        /// <summary>
        /// clears the cache, after a update from a resource for example
        /// </summary>
        public static void ClearCache()
        {
            foreach (var dic in _resourceCache)
            {
                dic.Value.ClearCache();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="route"></param>
        /// <param name="key"></param>
        public static void RemoveKey(string route, string key)
        {
            foreach (var resourceRoute in _resourceCache.Keys)
            {
                if (route == Resource.ROUTE_SEPARATOR || resourceRoute == route.ToLowerInvariant() || resourceRoute.StartsWith(route.ToLowerInvariant()))
                    _resourceCache[resourceRoute].RemoveKey(key);
            }
        }
    }
}
