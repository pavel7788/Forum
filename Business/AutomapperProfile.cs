using AutoMapper;
using Business.Models;
using Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<User, UserModel>();
                //.ForMember(um => um.PostsIds, opt => opt.MapFrom(u => u.Posts.Select(p => p.Id)))
                //.ForMember(um => um.CommentsIds, opt => opt.MapFrom(u => u.Comments.Select(c => c.Id)));
            CreateMap<UserModel, User>();

            CreateMap<Post, PostModel>()
                .ForMember(pm => pm.UserName, opt => opt.MapFrom(p => p.User.UserName));
                //.ForMember(pm => pm.PostCommentsIds, opt => opt.MapFrom(p => p.PostComments.Select(c => c.Id)));
            CreateMap<PostModel, Post>();

            CreateMap<Comment, CommentModel>()
               .ForMember(cm => cm.UserName, opt => opt.MapFrom(c => c.User.UserName))
               .ForMember(cm => cm.Title, opt => opt.MapFrom(c => c.Post.Title));
            CreateMap<CommentModel, Comment>();

        }
    }
}
