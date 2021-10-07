
using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using GrpcAgent;
using PAD.LAB1.Shared.Models;

namespace PAD.LAB1.Broker.Utils
{
    public class SharedMapperProfile : Profile
    {
        public SharedMapperProfile()
        {
            CreateMap<MessageModel, Message>()
                .ForMember(x => x.DateTime, opt => opt.MapFrom(src => Timestamp.FromDateTime(src.DateTime)));
            CreateMap<Message, MessageModel>()
                .ForMember(x => x.DateTime, opt => opt.MapFrom(src => src.DateTime.ToDateTime()));
            CreateMap<MemberModel, Member>().ReverseMap();
            CreateMap<MessageInfoModel, MessageInfo>().ReverseMap();
        }
    }
}
