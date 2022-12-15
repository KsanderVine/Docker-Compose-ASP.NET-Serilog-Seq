namespace UserBlogApp.Dtos
{
    public class PostReadDto
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string Content { get; set; } = string.Empty;
    }
}
