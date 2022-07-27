using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserRepository: IRepositoryDetails<User>
    {
        void DeleteCommentsByUserId(string id);
        void DeleteCommentsByUser(User entity);
        Task<IEnumerable<Post>> GetPostsWithDetailsByUserIdAsync(string id);
        Task<IEnumerable<Comment>> GetCommentsWithDetailsInUserPostAsync (string userId, int postId);
    }
}
