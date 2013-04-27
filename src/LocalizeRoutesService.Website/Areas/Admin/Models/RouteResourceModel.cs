using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using IHomer.Services.LocalizeRoutes.Entities;
using System.Web.Mvc;
using System.Collections.Generic;

namespace LocalizeRoutesService.Website.Areas.Admin.Models
{
    public class RouteResourceModel
    {
        [Required]
        public int ResourceID { get; set; }
        [Required]
        public int ResourceType { get; set; }
        [Required]
        public string ResourceKey { get; set; }
        [Required]
        public string ResourceRoute { get; set; }
        public string Value { get; set; }
    }
}