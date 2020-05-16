using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAPI.Model
{
    public class Publication
    {
        public int PublicationId { get; set; }
        public string Contenu { get; set; }
        public DateTime DatePublication { get; set; }

        public Publication()
        {
            DatePublication = DateTime.Now;
        }

        [ForeignKey("ClassId")]
        public virtual Class ClassIdClassNavigation { get; set; }
    }
}
