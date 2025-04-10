namespace WebApp.Models
{
    public enum ReqHeadFieldType
    {
        Accept,
        Crawler,
        Device,
        Encode,
        Language,
        Referer,
        UsrAgent,
    }

    public class RequestHeader
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public ReqHeadFieldType Field { get; set; }
        public string Text { get; set; }
    }
}
