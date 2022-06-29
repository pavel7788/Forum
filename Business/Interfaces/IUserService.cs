using Business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserService
    {
        Task DeleteCommentsByUserIdAsync(string id);        
        Task<IEnumerable<UserModel>> GetAllWithDetailsAsync();
        Task<UserModel> GetByIdWithDetailsAsync(string id);
        Task<IEnumerable<PostModel>> GetPostsWithDetailsByUserIdAsync(string id);
        Task<IEnumerable<CommentModel>> GetCommentsWithDetailsInUserPostAsync (string userId, int postId);
    }
}
