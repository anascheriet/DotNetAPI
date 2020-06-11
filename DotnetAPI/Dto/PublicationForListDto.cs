using System;
using System.Collections.Generic;
using DotnetAPI.Model;

namespace DotnetAPI.Dto
{
    public class PublicationForListDto
    {
        public int PublicationId { get; set; }
        public string Content { get; set; }
        public ICollection<AttachmentForListDto> Attachements { get; set; } //Image, Video, File...
        //public virtual ICollection<Comment> Comments { get; set; }
        public DateTime DatePublication { get; set; }
        public UserForListDto Owner { get; set; } //Owner of publication
        public ClassForListDto Class { get; set; }
    }
}