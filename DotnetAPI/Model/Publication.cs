using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Model
{
    public class Publication
    {
        public int PublicationId { get; set; }
        public string Content { get; set; }
        public ICollection<Attachement> Attachements { get; set; } //Image, Video, File...
        public ICollection<Comment> Comments { get; set; }
        public DateTime DatePublication { get; set; }
        public int ClassId { get; set; }
        public int AppUserId { get; set; } //Owner of publication

        public Publication()
        {
            DatePublication = DateTime.Now;
        }

        [ForeignKey("ClassId")]
        public virtual Class ClassIdClassNavigation { get; set; }
    }
}
