using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserBlogApp.Models;

namespace UserBlogApp.Data
{
    public class PostsRepository : IPostsRepository
    {
        private readonly ILogger<PostsRepository> _logger;
        private readonly AppDbContext _context;

        public PostsRepository(
            ILogger<PostsRepository> logger,
            AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void Create(Post entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.Posts.Add(entity);
        }

        public async Task CreateAsync(Post entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Posts.AddAsync(entity);
        }

        public void Delete(Post entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.Posts.Remove(entity);
        }

        public IEnumerable<Post?>? GetAll()
        {
            return _context.Posts;
        }

        public Post? GetById(Guid id)
        {
            return _context.Posts.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Post?>? GetAllIncludeAuthor()
        {
            return _context.Posts.Include(p => p.Author);
        }

        public IEnumerable<Post?>? GetPostsByAuthorId(Guid userId)
        {
            return _context.Posts
                .Where(p => p.AuthorId == userId)
                .OrderBy(e => e.CreatedAt);
        }

        public IEnumerable<Post> GetWhere(Expression<Func<Post, bool>> predicate)
        {
            return _context.Posts.Where(predicate);
        }

        public bool Save()
        {
            try
            {
                var stateEntires = _context.SaveChanges();
                return stateEntires > 0;
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while save changes in database");
                return false;
            }
        }

        public async Task<bool> SaveAsync()
        {
            try
            {
                var stateEntires = await _context.SaveChangesAsync();
                return stateEntires > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while save changes in database");
                return false;
            }
        }

        public bool Update(Post entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.Entry(entity).State = EntityState.Modified;
            return Save();
        }

        public async Task<bool> UpdateAsync(Post entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.Entry(entity).State = EntityState.Modified;
            return await SaveAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
