using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<PostModel>> GetPostsWithDetailsByUserIdAsync(string id)
        {
            var collection = await _uow.UserRepository.GetPostsWithDetailsByUserIdAsync(id);
            return _mapper.Map<IEnumerable<PostModel>>(collection);
        }

        public async Task<IEnumerable<CommentModel>> GetCommentsWithDetailsInUserPostAsync (string userId, int postId)
        {
            var collection = await _uow.UserRepository.GetCommentsWithDetailsInUserPostAsync(userId, postId);
            return _mapper.Map<IEnumerable<CommentModel>>(collection);
        }

        public async Task<IEnumerable<UserModel>> GetAllWithDetailsAsync()
        {
            var collection = await _uow.UserRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<UserModel>>(collection);
        }
        public async Task<UserModel> GetByIdWithDetailsAsync(string id)
        {
            var user  = await _uow.UserRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<UserModel>(user);
        }

        public async Task DeleteCommentsByUserIdAsync(string id)
        {
            _uow.UserRepository.DeleteCommentsByUserId(id);
            await _uow.SaveAsync();
        }

    }
}
