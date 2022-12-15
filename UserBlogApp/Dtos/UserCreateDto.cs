using System.ComponentModel.DataAnnotations;

namespace UserBlogApp.Dtos
{
    public class UserCreateDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;
    }
}
