namespace DotnetAPI.Dto
{
    public class AttachmentForListDto
    {
        public int AttachmentId { get; set; }
        public string path { get; set; }
        public PublicationForListDto Publication { get; set; }
    }
}