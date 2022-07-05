    using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public PostService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task AddAsync(PostModel model)
        {
            if (model is null)
            {
                throw new ForumException("Invalid post model");
            }
            if (string.IsNullOrEmpty(model.Title))
            {
                throw new ForumException("Post title can not be empty or null.");
            }
            if (string.IsNullOrEmpty(model.Summary))
            {
                throw new ForumException("Post title can not be empty or null.");
            }
            if (string.IsNullOrEmpty(model.Content))
            {
                throw new ForumException("Post info can not be empty or null.");
            }         
            model.PublishDate = DateTime.Now;
            var item = _mapper.Map<PostModel, Post>(model);
            await _uow.PostRepository.AddAsync(item);
            await _uow.SaveAsync();
        }

        public async Task DeleteAsync<T>(T modelId)
        {
            await _uow.PostRepository.DeleteByIdAsync(modelId);
            await _uow.SaveAsync();
        }

        public async Task<IEnumerable<PostModel>> GetAllAsync()
        {
            var collection = await _uow.PostRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<PostModel>>(collection);
        }       
        public async Task<PostModel> GetByIdAsync<T>(T id)
        {
            var item = await _uow.PostRepository.GetByIdAsync(id);
            return _mapper.Map<PostModel>(item);
        }

        public async Task UpdateAsync(PostModel model)
        {
            if (model is null)
            {
                throw new ForumException("Invalid post model");
            }
            if (string.IsNullOrEmpty(model.Title))
            {
                throw new ForumException("Post title can not be empty or null.");
            }
            if (string.IsNullOrEmpty(model.Summary))
            {
                throw new ForumException("Post title can not be empty or null.");
            }
            if (string.IsNullOrEmpty(model.Content))
            {
                throw new ForumException("Post info can not be empty or null.");
            }          
            model.PublishDate = DateTime.Now;
            _uow.PostRepository.Update(_mapper.Map<Post>(model));
            await _uow.SaveAsync();
        }

        public async Task<IEnumerable<CommentModel>> GetCommentsByPostIdAsync(int id)
        {
            var collection = await _uow.PostRepository.GetCommentsByPostIdAsync(id);
            return _mapper.Map<IEnumerable<CommentModel>>(collection);
        }

        public async Task<IEnumerable<PostModel>> GetAllWithDetailsAsync()
        {
            var collection = await _uow.PostRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<PostModel>>(collection);
        }

        public async Task<PostModel> GetByIdWithDetailsAsync<T>(T id)
        {
            var item = await _uow.PostRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<PostModel>(item);
        }
    }
}
