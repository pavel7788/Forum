using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
    public class ForumDbContext: IdentityDbContext<User> //IdentityDbContext
    {
        public ForumDbContext() {}

        public ForumDbContext(DbContextOptions<ForumDbContext> options)
           : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=PAVEL-PC\PAVEL_SERVER;Database=ForumDB;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(t => t.Posts)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(p => p.Post)
                .WithMany(t => t.Comments)
                .OnDelete(DeleteBehavior.Cascade);

            /*modelBuilder.Entity<Comment>()
                .HasOne(p => p.User)
                .WithMany(t => t.Comments)
                .OnDelete(DeleteBehavior.Cascade);*/

        }




    }
}
