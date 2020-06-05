namespace DotnetAPI.Model
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public int AppUserId { get; set; }
        public int PublicationId { get; set; }
    }
}