using System.Collections.Generic;

namespace DotnetAPI.Model
{
    public class Class
    {
        public int ClassId { get; set; }
        public string Name { get; set; }
        public string Branch { get; set; }
        public string Grade { get; set; }
        public string InvitationCode { get; set; }
        public ICollection<ClassPending> WaitingList { get; set; }
        public int AppUserId { get; set; }
        public ICollection<Publication> publications { get; set; }

        public Class()
        {
            //Generate random code
            InvitationCode = "SecretRandomCode";
        }

    }
}
