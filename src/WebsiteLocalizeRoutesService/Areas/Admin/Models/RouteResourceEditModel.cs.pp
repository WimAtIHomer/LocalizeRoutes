using System.ComponentModel.DataAnnotations;

namespace $rootnamespace$.Areas.Admin.Models
{
    public class RouteResourceEditModel
    {
        [Required]
        public int ResourceID { get; set; }
        [Required]
        public int ResourceType { get; set; }
        [Required]
        public string ResourceRoute { get; set; }
        [Required]
        public string ResourceKey { get; set; }
        [Required]
        public int LanguageID { get; set; }

        [DataType(DataType.Html)]
        public string CKEditor { get; set; }
        public string TextEditor { get; set; }

        public bool ClearCache { get; set; }
    }
}