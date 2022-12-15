namespace UserBlogApp.Models
{
    public class Post : BaseModel
    {
        public Guid AuthorId { get; set; }
        public User? Author { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
