using System;
using System.Collections.Generic;
using DotnetAPI.Model;

namespace DotnetAPI.Dto
{
    public class PublicationAddDto
    {
        public int ClassId { get; set; }
        public string Content { get; set; }
        public List<String> Files { get; set; }
    }
}