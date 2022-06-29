using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ForumDbContext _context;
        private readonly Repository<Post> _repoPosts;
        public PostRepository(ForumDbContext context)
        {
            _context = context;
            _repoPosts = new Repository<Post>(context);
        }

        public async Task AddAsync(Post entity)
        {
            await _repoPosts.AddAsync(entity);
        }

        public void Delete(Post entity)
        {
            var delComments = _context.Comments
                  .Include(p => p.User)
                  .Include(p => p.Post)
                  .Where(p => p.PostId == entity.Id);

            if (delComments != null)
                _context.Comments.RemoveRange(delComments);

            _repoPosts.Delete(entity);
        }

        public async Task DeleteByIdAsync<T>(T id)
        {
            await _repoPosts.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await _repoPosts.GetAllAsync();
        }
        public async Task<Post> GetByIdAsync<T>(T id)
        {
            return await _repoPosts.GetByIdAsync(id);
        }

        public void Update(Post entity)
        {
            _repoPosts.Update(entity);
        }

        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int id)
        {
            return await _context.Comments
                  .Include(p => p.User)
                  .Include(p => p.Post)
                  .Where(p => p.PostId == id)
                  .AsNoTracking()
                  .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetAllWithDetailsAsync()
        {
            return await _context.Posts
                  .Include(p => p.User)
                  .Include(p => p.Comments)
                  .AsNoTracking()
                  .ToListAsync();
        }

        public async Task<Post> GetByIdWithDetailsAsync<T>(T id)
        {
            int postId = Convert.ToInt32(id);

            return await _context.Posts
                  .Include(p => p.User)
                  .Include(p => p.Comments)
                  .Where(p=>p.Id==postId)
                  .AsNoTracking()
                  .SingleAsync();
        }
    }
}
