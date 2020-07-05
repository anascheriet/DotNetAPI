using System.Collections.Generic;
using DotnetAPI.Model;

namespace DotnetAPI.Dto
{
    public class UserForListDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string LName { get; set; }
        public string FName { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }

    }
}