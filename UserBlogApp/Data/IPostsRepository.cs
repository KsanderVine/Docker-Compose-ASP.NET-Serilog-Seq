using UserBlogApp.Models;

namespace UserBlogApp.Data
{
    public interface IPostsRepository : IRepository<Guid, Post>
    {
        IEnumerable<Post?>? GetPostsByAuthorId(Guid userId);
        IEnumerable<Post?>? GetAllIncludeAuthor();
    }
}
