using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CommentService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task AddAsync(CommentModel model)
        {
            if (model is null)
            {
                throw new ForumException("Invalid comment model");
            }
            if (string.IsNullOrEmpty(model.Content))
            {
                throw new ForumException("Comment can not be empty or null.");
            }
            model.PublishDate = DateTime.Now;
            var item = _mapper.Map<CommentModel, Comment>(model);
            await _uow.CommentRepository.AddAsync(item);
            await _uow.SaveAsync();
        }

        public async Task DeleteAsync<T>(T modelId)
        {
            await _uow.CommentRepository.DeleteByIdAsync(modelId);
            await _uow.SaveAsync();
        }

        public async Task<IEnumerable<CommentModel>> GetAllAsync()
        {
            var collection = await _uow.CommentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CommentModel>>(collection);
        }      

        public async Task<CommentModel> GetByIdAsync<T>(T id)
        {
            var item = await _uow.CommentRepository.GetByIdAsync(id);
            return _mapper.Map<CommentModel>(item);
        }        

        public async Task UpdateAsync(CommentModel model)
        {
            if (model is null)
            {
                throw new ForumException("Invalid comment model");
            }
            if (string.IsNullOrEmpty(model.Content))
            {
                throw new ForumException("Comment can not be empty or null.");
            }
            model.PublishDate = DateTime.Now;
            _uow.CommentRepository.Update(_mapper.Map<Comment>(model));
            await _uow.SaveAsync();
        }

        public async Task<IEnumerable<CommentModel>> GetAllWithDetailsAsync()
        {
            var collection = await _uow.CommentRepository.GetAllWithDetailsAsync();
            return _mapper.Map<IEnumerable<CommentModel>>(collection);
        }

        public async Task<CommentModel> GetByIdWithDetailsAsync<T>(T id)
        {
            var item = await _uow.CommentRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<CommentModel>(item);
        }
    }
}
