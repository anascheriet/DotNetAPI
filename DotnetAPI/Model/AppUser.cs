using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Model
{
    public class AppUser : IdentityUser
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Status { get; set; }

        public ICollection<Class> Classes { get; set; }
    }
}
