using System.ComponentModel.DataAnnotations;

namespace DotnetAPI.Dto
{
    public class UserForRegisterDto
    {

        [Required]
        public string UserName { get; set; }
        [Required]
        public string password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        public string FName { get; set; }
        [Required]
        public string Status { get; set; }
    }
}