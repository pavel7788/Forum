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
    public class UserRepository : IUserRepository
    {
        private readonly ForumDbContext _context;
        public UserRepository(ForumDbContext context)
        {
            _context = context;
        }       
        public async Task<IEnumerable<User>> GetAllWithDetailsAsync()
        {
            return await _context.Users
                 .Include(p => p.Posts)
                 .Include(p => p.Comments)
                 .AsNoTracking()
                 .ToListAsync();
        }
        public async Task<User> GetByIdWithDetailsAsync<T>(T id)
        {
            return await _context.Users                
                .Include(p => p.Posts)
                .Include(p => p.Comments)
                .Where(p => p.Id == id.ToString())
                .AsNoTracking()
                .SingleAsync();
        }
        public async Task<IEnumerable<Post>> GetPostsWithDetailsByUserIdAsync(string id)
        {
            return await _context.Posts
                  .Include(p => p.User)
                  .Include(p => p.Comments)
                  .Where(p => p.UserId == id)
                  .AsNoTracking()
                  .ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsWithDetailsInUserPostAsync(string userId, int postId)
        {
            var collection = await _context.Comments
                  .Include(p => p.User)
                  .Include(p => p.Post)
                  .Where(p => p.UserId == userId)
                  .AsNoTracking()
                  .ToListAsync();
            
            return collection.Where(p => p.PostId == postId);
        }

        public void DeleteCommentsByUserId(string id)
        {
            var delComments = _context.Comments
                 .Include(p => p.User)
                 .Include(p => p.Post)
                 .Where(p => p.UserId == id.ToString());

            if (delComments != null)
                _context.Comments.RemoveRange(delComments);
        }

        public void DeleteCommentsByUser(User entity)
        {
            var delComments = _context.Comments
                 .Include(p => p.User)
                 .Include(p => p.Post)
                 .Where(p => p.UserId == entity.Id);

            if (delComments != null)
                _context.Comments.RemoveRange(delComments);
        }        
    }
}
