using DotnetAPI.Model;

namespace DotnetAPI.Dto
{
    public class ClassForListDto
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
        public string Branch { get; set; }
        public string Grade { get; set; }
        public string InvitationCode { get; set; }
        public UserForListDto Owner { get; set; }
    }
}