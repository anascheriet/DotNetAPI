using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DotnetAPI.Model
{
    public class AppUser : IdentityUser<int>
    {
        public string LName { get; set; }
        public string FName { get; set; }
        public string Status { get; set; }

        public ICollection<Class> Classes { get; set; }
    }
}
