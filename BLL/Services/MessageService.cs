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
    public class MessageService : IMessageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<MessageDTO> CreateAsync(MessageDTO model)
        {
            if (model == null) throw new NotFoundException("Message can't be created");

            var message = _mapper.Map<Message>(model);
            await _unitOfWork.MessageRepository.CreateAsync(message);
            await _unitOfWork.SaveAsync();
            model.Id = message.Id;
            return model;
        }

        public async Task<IEnumerable<MessageDTO>> GetAllMessages()
        {
            var messages = await _unitOfWork.MessageRepository.GetAllAsync();
            if (messages == null) throw new NotFoundException($"This messages wasn't found");

            var result = _mapper.Map<IEnumerable<MessageDTO>>(messages);
            return result;
        }

        public async Task<MessageDTO> GetByIdAsync(int id)
        {
            if (id <= 0) throw new ForumProjectException("Value of id must be positive");
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);

            if (message == null) throw new NotFoundException("This message wasn't found");
            return _mapper.Map<MessageDTO>(message);
        }

        public IEnumerable<MessageDTO> FindByUserId(int userId)
        {
            if (userId <= 0) throw new ForumProjectException("Value of id must be positive");

            var messages = _unitOfWork.MessageRepository.GetAll().Where(t => t.UserId == userId);
            return _mapper.Map<IQueryable<MessageDTO>>(messages);
        }

        public IEnumerable<MessageDTO> FindAllByUserId(int userId)
        {
            if (userId <= 0) throw new ForumProjectException("Value of id must be positive");

            var messages = _unitOfWork.MessageRepository.FindAllByUserId(userId);
            if (messages == null) throw new NotFoundException($"This topics wasn't found");

            var result = _mapper.Map<IEnumerable<MessageDTO>>(messages);
            return result;
        }

        public async Task UpdateAsync(MessageDTO model, int id)
        {
            var messageUpdate = await _unitOfWork.MessageRepository.GetByIdAsync(id);

            if (messageUpdate == null) throw new NotFoundException("Message not found");

            messageUpdate.Text = model.Text;

            _unitOfWork.MessageRepository.Update(messageUpdate);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var message = await _unitOfWork.MessageRepository.GetByIdAsync(id);
            if (message == null) throw new NotFoundException("Message not found");

            _unitOfWork.MessageRepository.Remove(message);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<MessageDTO>> GetByTopicIdWithDetailsAsync(int topicId)
        {
            if (topicId <= 0) throw new ForumProjectException("Value of id must be positive");

            var messages = await _unitOfWork.MessageRepository.GetByTopicIdWithDetailsAsync(topicId);
            if (messages == null) throw new NotFoundException("Messages not found");

            var result = _mapper.Map<IEnumerable<MessageDTO>>(messages);
            return result;
        }
    }
}