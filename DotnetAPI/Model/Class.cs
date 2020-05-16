using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Model
{
    public class Class
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
        public string Grade { get; set; }
        public string Branch { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUserIDAppUserNavigation { get; set; }

        public virtual ICollection<Publication> Publication { get; set; }
    }
}
