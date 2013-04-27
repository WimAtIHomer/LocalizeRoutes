using System;
using System.Collections.Generic;
using System.Linq;
using IHomer.Services.LocalizeRoutes.Context;
using IHomer.Services.LocalizeRoutes.Entities;

namespace IHomer.Services.LocalizeRoutes.Repositories
{
    public class ResourceRepository
    {
        /// <summary>
        /// Adds a resource to the Localization Table
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="languageID"></param>
        /// <param name="route"></param>
        /// <param name="type"></param>
        public int AddResource(string key, string value, int languageID, string route, ResourceType type)
        {
            using (var entities = new ResourceDB())
            {
                var lan = (from l in entities.Languages where l.ID == languageID select l).FirstOrDefault();

                var res = new Resource
                              {
                                  Key = key,
                                  Route = route,
                                  Value = value,
                                  Language = lan,
                                  ResourceType = type,
                                  CreationDate = DateTime.Now
                              };

                entities.Resources.Add(res);
                return entities.SaveChanges();
            }
        }

        /// <summary>
        /// Gets a specific resource in the Localization Table
        /// </summary>
        /// <param name="key"></param>
        /// <param name="language"></param>
        /// <param name="route"></param>
        public Resource GetResource(string key, string route, string language)
        {
            using (var entities = new ResourceDB())
            {
                var res = (from r in entities.Resources.Include("Language") 
                           where r.Language.Name == language 
                           && r.Route == route && r.Key == key 
                           select r).FirstOrDefault();

                return res;
            }
        }

        /// <summary>
        /// Gets a specific resource in the Localization Table
        /// </summary>
        /// <param name="key"></param>
        /// <param name="route"></param>
        /// <param name="languageID"></param>
        public Resource GetResource(string key, string route, int languageID)
        {
            using (var entities = new ResourceDB())
            {
                var res = (from r in entities.Resources.Include("Language") 
                           where r.Language.ID == languageID 
                           && r.Route == route 
                           && r.Key == key 
                           select r).FirstOrDefault();

                return res;
            }
        }

        /// <summary>
        /// Updates a resource in the Localization Table
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="languageID"></param>
        /// <param name="route"></param>
        /// <param name="type"></param>
        public int UpdateResource(int id, string key, string value, int languageID, string route, ResourceType type)
        {
            using (var entities = new ResourceDB())
            {
                var res = (from r in entities.Resources.Include("Language") 
                           where r.ID == id 
                           select r).First();

                res.Value = value;
                res.Key = key;
                res.ResourceType = type;
                res.LanguageID = languageID;
                res.Route = route;

                try
                {
                    return entities.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a specific set of resources for a given culture and 'resource set' which
        /// in this case is just the virtual directory and culture.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        public IList<Resource> GetResources(string language, string route)
        {
            using (var entities = new ResourceDB())
            {
                var lan = (from l in entities.Languages 
                           where l.Name == language 
                           select l).FirstOrDefault() 
                          ??
                          (from l in entities.Languages 
                           where l.Default == true 
                           select l).FirstOrDefault();

                var res = (from r in entities.Resources 
                           where r.Language.ID == lan.ID 
                           && r.Route == route 
                           select r).ToList();

                return res;
            }
        }


        /// <summary>
        /// Returns a specific set of resources for a given culture and 'resource set' which
        /// in this case is just the virtual directory and culture.
        /// </summary>
        /// <param name="language"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public List<Resource> GetResourcesByPageAndLanguage(string page, string language)
        {
            using (var entities = new ResourceDB())
            {
                return (from r in entities.Resources 
                        where r.Route == page 
                        && r.Language.Name == language 
                        select r).ToList();
            }
        }

        /// <summary>
        /// Returns a specific resources
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Resource GetResourceByID(int id)
        {
            using (var entities = new ResourceDB())
            {
                return (from r in entities.Resources.Include("Language") 
                        where r.ID == id 
                        select r).FirstOrDefault();
            }
        }

        /// <summary>
        /// Returns a specific set of resources for a given culture
        /// </summary>
        /// <param name="language"></param>
        /// <returns></returns>
        public List<Resource> GetResourcesByLanguage(string language)
        {
            using (var entities = new ResourceDB())
            {
                return (from r in entities.Resources 
                        where r.Language.Name == language 
                        orderby r.Route ascending
                        , r.Key ascending 
                        select r).ToList();
            }
        }

        /// <summary>
        /// Returns a specific set of resources for a given culture
        /// </summary>
        /// <param name="language"></param>
        /// <param name="page"></param>
        /// <param name="listCode"></param>
        /// <returns></returns>
        public List<Resource> GetResourcesByLanguage(string language, string page, string listCode)
        {
            using (var entities = new ResourceDB())
            {
                var lan = (from l in entities.Languages 
                           where l.Name == language 
                           select l).FirstOrDefault() 
                           ??
                          (from l in entities.Languages 
                           where l.Default == true 
                           select l).FirstOrDefault();

                var query = (from r in entities.Resources 
                             where r.LanguageID == lan.ID 
                             && r.Route == page 
                             && r.Key.StartsWith(listCode) 
                             && r.Key != listCode 
                             orderby r.Route ascending
                             , r.Key ascending 
                             select r);
                
                return query.ToList();
            }
        }

        /// <summary>
        /// returns the distinct list of routes in the resource table
        /// </summary>
        /// <returns></returns>
        public List<string> GetRoutes()
        {
            using (var entities = new ResourceDB())
            {
                return (from r in entities.Resources 
                        orderby r.Route ascending 
                        select r.Route).Distinct().ToList();
            }
        }

        /// <summary>
        /// retrieves the dictionary of languages, with key culturecode, from the database, if this list is empty inserts the default language 'en' 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Language> GetLanguages()
        {
            using (var entities = new ResourceDB())
            {
                var list = (from l in entities.Languages 
                            orderby l.Name ascending
                            select l).ToDictionary(l => l.Name);

                if (list.Count == 0)
                {
                    var en = new Language
                                 {
                                     Name = "en",
                                     EnglishName = "English",
                                     NativeName = "English",
                                     Default = true
                                 };
                    entities.Languages.Add(en);
                    entities.SaveChanges();

                    list = (from l in entities.Languages
                            orderby l.Name ascending 
                            select l).ToDictionary(l => l.Name);
                }
                return list;
            }
        }

        /// <summary>
        /// retrieves the list of languages from the database, if this list is empty inserts the default language 'en' 
        /// </summary>
        /// <returns></returns>
        public Language GetDefaultLanguage()
        {
            using (var entities = new ResourceDB())
            {
                var language = (from l in entities.Languages
                                where l.Default == true
                                select l).FirstOrDefault();

                if (language == null)
                {
                    var en = new Language
                    {
                        Name = "en",
                        EnglishName = "English",
                        NativeName = "English",
                        Default = true
                    };
                    entities.Languages.Add(en);
                    entities.SaveChanges();

                    return en;
                }
                return language;
            }
        }

        public void DeleteResource(int id)
        {
            using (var entities = new ResourceDB())
            {
                var res = (from r in entities.Resources
                           where r.ID == id
                           select r).FirstOrDefault();
                entities.Resources.Remove(res);
                entities.SaveChanges();
            }
        }
    }
}
