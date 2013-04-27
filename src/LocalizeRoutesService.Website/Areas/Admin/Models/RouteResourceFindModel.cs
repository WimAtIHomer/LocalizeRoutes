using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using IHomer.Services.LocalizeRoutes.Entities;
using System.Web.Mvc;
using System.Collections.Generic;

namespace LocalizeRoutesService.Website.Areas.Admin.Models
{
    public class RouteResourceFindModel
    {
        public RouteResourceFindModel()
        {
            Type = new SelectList((from ResourceType e in Enum.GetValues(typeof(ResourceType)) select new { ID = (int)e, Name = e.ToString() }), "ID", "Name");
        }

        public string ID { get; set; }
        [Required]
        public string Route { get; set; }
        [Required]
        public int LanguageID { get; set; }
        [Required]
        public int ResourceID { get; set; }
        [Required]
        public int ResourceType { get; set; }
        [Required]
        public string ResourceKey { get; set; }
        [Required]
        public string ResourceRoute { get; set; }
        public string Value { get; set; }
        public string SaveError { get; set; }
        public bool ClearCache { get; set; }
        public SelectList Type { get; set; }
        public SelectList Languages { get; set; }
        public SelectList Routes { get; set; }
        public List<Resource> Resources { get; set; }
    }
}