using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Entities
{
    public class Post
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Post Title")]
        public string Title { get; set; }
        [Required]
        [MaxLength(100)]
        [Display(Name = "Post Summary")]
        public string Summary { get; set; }
        [Required]
        [MaxLength(1000)]
        [Display(Name = "Post Content")]
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }
}
