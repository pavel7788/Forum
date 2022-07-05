using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class PostModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public string UserId { get; set; }
        public virtual string UserName { get; set; }
        //public virtual ICollection<int> CommentsIds { get; set; }
        public virtual ICollection<CommentModel> Comments { get; set; }
    }
}
