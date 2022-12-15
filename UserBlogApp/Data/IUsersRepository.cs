using UserBlogApp.Models;

namespace UserBlogApp.Data
{
    public interface IUsersRepository : IRepository<Guid, User>
    {
        IEnumerable<User?>? GetAllIncludePosts();
    }
}
