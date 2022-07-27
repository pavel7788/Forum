using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsBanned { get; set; }
        public string UserRoles { get; set; }
        public virtual ICollection<PostModel> Posts { get; set; }
        //public virtual ICollection<int> PostsIds { get; set; }
        public virtual ICollection<CommentModel> Comments { get; set; }
        //public virtual ICollection<int> CommentsIds { get; set; }
    }
}
