namespace DotnetAPI.Model
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public int OwnerId { get; set; }
        public virtual AppUser Owner { get; set; }
        public int PublicationId { get; set; }
        public virtual Publication Publication { get; set; }
    }
}