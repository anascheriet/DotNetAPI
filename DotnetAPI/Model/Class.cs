using System;
using System.Collections.Generic;
using System.Linq;

namespace DotnetAPI.Model
{
    public class Class
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
        public string Branch { get; set; }
        public string Grade { get; set; }
        public string InvitationCode { get; set; }
        public virtual ICollection<ClassAppUser> ClassMembers { get; set; }
        public virtual AppUser Owner { get; set; }
        public virtual ICollection<Publication> publications { get; set; }

        public Class()
        {

            //Generate random invitation code
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[8];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            InvitationCode = new String(stringChars);
            
        }

    }
}
