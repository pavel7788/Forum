using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        public string Info { get; set; }
        public DateTime Date { get; set; }
        public int PostId { get; set; }
        public virtual string Title { get; set; }
        public string UserId { get; set; }
        public virtual string UserName { get; set; }
    }
}
