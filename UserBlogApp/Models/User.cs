namespace UserBlogApp.Models
{
    public class User : BaseModel
    {
        public string Username { get; set; } = string.Empty;
        public IEnumerable<Post> Posts { get; set; } = new List<Post>();
    }
}
