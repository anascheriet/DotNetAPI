using System.Text.Json.Serialization;

namespace DotnetAPI.Model
{
    public class ClassAppUser
    {

        public int ClassId { get; set; }
        public virtual Class Class { get; set; }

        public int MemberId { get; set; }
        public virtual AppUser Member { get; set; }

        public bool verified { get; set; }

        public ClassAppUser()
        {
            verified = false;
        }
    }
}