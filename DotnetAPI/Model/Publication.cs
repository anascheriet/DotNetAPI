using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Model
{
    public class Publication
    {
        public int PublicationId { get; set; }
        public string Content { get; set; }
        public virtual ICollection<Attachement> Attachements { get; set; } //Image, Video, File...
        public virtual ICollection<Comment> Comments { get; set; }
        public DateTime DatePublication { get; set; }
        public virtual Class Class { get; set; }
        public virtual AppUser AppUser { get; set; } //Owner of publication

        public Publication()
        {
            DatePublication = DateTime.Now;
        }


    }
}
