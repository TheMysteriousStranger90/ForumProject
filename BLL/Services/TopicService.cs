using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BLL.DTO;
using BLL.Exceptions;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;

namespace BLL.Services
{
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TopicService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<TopicDTO> CreateAsync(TopicDTO model)
        {
            if (model == null) throw new NotFoundException("Topic can't be created");
            
            var topic = _mapper.Map<Topic>(model);
            await _unitOfWork.TopicRepository.CreateAsync(topic);
            await _unitOfWork.SaveAsync();
            model.Id = topic.Id;
            return model;
        }

        public IEnumerable<TopicDTO> GetAllTopics()
        {
            var topics = _unitOfWork.TopicRepository.GetAllAsync();
            if (topics == null) throw new NotFoundException($"This topics wasn't found");
            
            var result = _mapper.Map<IEnumerable<TopicDTO>>(topics);
            return result;
        }

        public async Task<IEnumerable<TopicDTO>> GetAllWithDetailsAsync()
        {
            var topics = await _unitOfWork.TopicRepository.GetAllWithDetailsAsync();
            if (topics == null) throw new NotFoundException($"This topics wasn't found");
            
            var result =  _mapper.Map<IEnumerable<TopicDTO>>(topics);
            return result;
        }

        public async Task<TopicDTO> GetByIdWithDetailsAsync(int id)
        {
            var topic = await _unitOfWork.TopicRepository.GetByIdWithDetailsAsync(id);
            if (topic == null) throw new NotFoundException($"This topic wasn't found");
            
            var result =  _mapper.Map<TopicDTO>(topic);
            return result;
        }

        public async Task<TopicDTO> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ForumProjectException("Value of id must be positive");
            var topic =  await _unitOfWork.TopicRepository.GetByIdAsync(id);
            
            if (topic  == null) throw new NotFoundException("This topic wasn't found");
            return _mapper.Map<TopicDTO>(topic);
        }

        public IEnumerable<TopicDTO> GetByUserId(int userId)
        {
            if (userId <= 0) throw new ForumProjectException("Value of id must be positive");
            
            var topics = _unitOfWork.TopicRepository.GetAll().Where(t => t.UserId == userId);
            return _mapper.Map<IQueryable<TopicDTO>>(topics);
        }

        public IEnumerable<TopicDTO> GetByCategoryId(int categoryId)
        {
            if (categoryId <= 0) throw new ForumProjectException("Value of id must be positive");
            
            var topics = _unitOfWork.TopicRepository.GetAll().Where(t => t.CategoryId == categoryId);
            return _mapper.Map<IEnumerable<TopicDTO>>(topics);
        }

        public async Task UpdateAsync(TopicDTO model, int id)
        {
            var topicUpdate = await _unitOfWork.TopicRepository.GetByIdAsync(id);
            
            if (topicUpdate == null) throw new NotFoundException("Topic not found");
            
            topicUpdate.Title = model.Title;

            _unitOfWork.TopicRepository.Update(topicUpdate);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var topic = await _unitOfWork.TopicRepository.GetByIdAsync(id);
            if (topic == null) throw new NotFoundException("Topic not found");
            
            _unitOfWork.TopicRepository.Remove(topic);
            await _unitOfWork.SaveAsync();
        }
    }
}