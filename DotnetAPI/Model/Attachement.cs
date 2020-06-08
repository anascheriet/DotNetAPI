namespace DotnetAPI.Model
{
    public class Attachement
    {
        public int AttachementId { get; set; }
        public string path { get; set; }
        public virtual Publication Publication { get; set; }
    }
}