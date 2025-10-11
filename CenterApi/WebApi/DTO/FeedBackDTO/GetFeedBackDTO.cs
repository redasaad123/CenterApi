namespace WebApi.DTO.FeedBackDTO
{
    public class GetFeedBackDTO
    {
        public string FeedBackId { get; set; }
        public string studentName { get; set; }

        public string? materailName { get; set; }
        public string message { get; set; }
    }
}
