using System.ComponentModel.DataAnnotations;

namespace IHomer.Services.LocalizeRoutes.Entities
{
    public class Language
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string EnglishName { get; set; }
        [Required]
        [StringLength(50)]
        public string NativeName { get; set; }
        [Required]
        public bool Default { get; set; }
    }
}
