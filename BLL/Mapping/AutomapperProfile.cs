using System.Collections.Specialized;
using System.Linq;
using AutoMapper;
using BLL.DTO;
using DAL.Entities;

namespace BLL.Mapping
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<User, SignUpDTO>().ReverseMap();
            CreateMap<Message, MessageDTO>()
                .ReverseMap();
            CreateMap<Topic, TopicDTO>()
                .ForMember(t => t.MessagesId, opt => opt.MapFrom(a => a.Messages.Select(x => x.Id)))
                .ReverseMap();
            CreateMap<Category, CategoryDTO>()
                .ForMember(s => s.TopicsId, opt => opt.MapFrom(section => section.Topics.Select(t => t.Id)))
                .ReverseMap();
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
        }
    }
}