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
        [MaxLength(1000)]
        [Display(Name = "Post Info")]
        public string Info { get; set; }
        public string PhotoPath { get; set; }
        public DateTime Date { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }
}
