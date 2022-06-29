using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ForumDbContext _context;
        private IUserRepository _userRepo;
        private IPostRepository _postRepo;
        private ICommentRepository _commentRepo;
        public UnitOfWork(ForumDbContext context)
        {
            _context = context;
        }
        public IUserRepository UserRepository => _userRepo ??= new UserRepository(_context);
        public IPostRepository PostRepository => _postRepo ??= new PostRepository(_context);
        public ICommentRepository CommentRepository => _commentRepo ??= new CommentRepository(_context);

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
