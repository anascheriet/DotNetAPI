using DotnetAPI.Model;

namespace DotnetAPI.Dto
{
    public class CommentForListDto
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public UserForListDto Owner { get; set; }
        public PublicationForListDto Publication { get; set; }
    }
}