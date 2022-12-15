using System.ComponentModel.DataAnnotations;

namespace UserBlogApp.Dtos
{
    public class PostCreateDto
    {
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}
