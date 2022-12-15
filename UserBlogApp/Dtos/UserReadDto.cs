namespace UserBlogApp.Dtos
{
    public class UserReadDto
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Username { get; set; } = string.Empty;
    }
}
