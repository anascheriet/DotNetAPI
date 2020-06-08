using System.ComponentModel.DataAnnotations;
using DotnetAPI.Model;

namespace DotnetAPI.Dto
{
    public class ClassForCrudDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Branch { get; set; }
        [Required]
        public string Grade { get; set; }

    }
}