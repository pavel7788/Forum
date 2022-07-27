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
    public class CommentRepository : ICommentRepository
    {
        private readonly ForumDbContext _context;
        private readonly Repository<Comment> _repo;
        public CommentRepository(ForumDbContext context)
        {
            _context = context;
            _repo = new Repository<Comment>(context);
        }
        public async Task AddAsync(Comment entity)
        {
            await _repo.AddAsync(entity);
        }

        public void Delete(Comment entity)
        {
            _repo.Delete(entity);
        }

        public async Task DeleteByIdAsync<T>(T id)
        {
            await _repo.DeleteByIdAsync(id); 
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }      

        public async Task<Comment> GetByIdAsync<T>(T id)
        {
            return await _repo.GetByIdAsync(id);
        }     

        public void Update(Comment entity)
        {
            _repo.Update(entity);
        }
        public async Task<IEnumerable<Comment>> GetAllWithDetailsAsync()
        {
            return await _context.Comments
                 .Include(p => p.User)
                 .Include(p => p.Post)
                 .AsNoTracking()
                 .ToListAsync();
        }

        public async Task<Comment> GetByIdWithDetailsAsync<T>(T id)
        {
            int commentId = Convert.ToInt32(id);

            return await _context.Comments
                 .Include(p => p.User)
                 .Include(p => p.Post)
                 .Where (p => p.Id == Convert.ToInt32(id))
                 .AsNoTracking()
                 .SingleAsync();
        }

    }
}
