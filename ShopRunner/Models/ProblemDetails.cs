namespace ShopRunner.Models
{
    public class ProblemDetails
    {
        public string Type { get; set; } = "https://tools.ietf.org/html/rfc7231#section-6.6.1";
        public string Title { get; set; } = "Something went wrong. ";
        public Dictionary<string,string[]> Errors { get; set; }
        public int Status { get; set; }
        public string Detail { get; set; } = "Please see th errors field for more details.";
    }
}
