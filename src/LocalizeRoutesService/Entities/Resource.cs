using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using IHomer.Services.LocalizeRoutes.Context;

namespace IHomer.Services.LocalizeRoutes.Entities
{
    public class Resource
    {
        public const string ROUTE_SEPARATOR = "/";

        [Key]
        public int ID { get; set; }
        [Required]
        public int LanguageID { get; set; }
        [Required]
        [StringLength(50)]
        public string Route { get; set; }
        [Required]
        [StringLength(50)]
        public string Key { get; set; }
        [Required]
        [StringLength(4000)]
        public string Value { get; set; }
        [Required]
        public ResourceType ResourceType { get; set; }

        public DateTime CreationDate { get; set; }
        
        [Required]
        public Language Language { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!validationContext.Items.ContainsKey("Context")) yield break;
            var resourceEntities = (ResourceDB)validationContext.Items["Context"];

            if (resourceEntities.Resources.Any(
                r => r.Route == Route && r.LanguageID == LanguageID && r.Key == Key && r.ID != ID))
            {
                yield return
                    new ValidationResult("ResourceUnique", new[] { "Code" });
            }
        }

        public static string GetUpperRoute(string route)
        {
            var index = route.LastIndexOf(ROUTE_SEPARATOR, StringComparison.Ordinal);
            return index == -1 ? ROUTE_SEPARATOR : route.Remove(index);
        }

        /// <summary>
        /// Split CamelCase and replace _ with space
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string SplitResourceCode(string input)
        {
            var i = 0;
            return string.Join(string.Empty, input
                .Select(c => new
                {
                    Character = c.ToString(CultureInfo.InvariantCulture),
                    Start = i++ == 0
                                || ((Char.IsUpper(input[i - 1]) || input[i-1] == '_')
                                    && ((!Char.IsUpper(input[i - 2]) || input[i-2] == '_')
                                        || (i < input.Length && !Char.IsUpper(input[i]) && input[i] != '_')))
                })
                .Select(x => x.Start ? " " + x.Character : x.Character)
                .ToArray())
                .Replace("_", "")
                .Trim();
        }
    }
}
