namespace DotnetAPI.Model
{
    public class Attachment
    {
        public int AttachmentId { get; set; }
        public string path { get; set; }
        public int PublicationId { get; set; }
        public virtual Publication Publication { get; set; }
    }
}