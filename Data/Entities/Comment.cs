using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(512)]
        [Display(Name = "Comment Content")]
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
    }
}
