using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using UserBlogApp.Models;

namespace UserBlogApp.Data
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ILogger<UsersRepository> _logger;
        private readonly AppDbContext _context;

        public UsersRepository(
            ILogger<UsersRepository> logger,
            AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void Create(User entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.Users.Add(entity);
        }

        public async Task CreateAsync(User entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            await _context.Users.AddAsync(entity);
        }

        public void Delete(User entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.Users.Remove(entity);
        }

        public IEnumerable<User?>? GetAll()
        {
            return _context.Users;
        }

        public IEnumerable<User?>? GetAllIncludePosts()
        {
            return _context.Users.Include(u => u.Posts);
        }

        public User? GetById(Guid id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<User?>? GetWhere(Expression<Func<User, bool>> predicate)
        {
            return _context.Users.Where(predicate);
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

        public bool Update(User entity)
        {
            if (entity is null)
                throw new ArgumentNullException(nameof(entity));

            _context.Entry(entity).State = EntityState.Modified;
            return Save();
        }

        public async Task<bool> UpdateAsync(User entity)
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
