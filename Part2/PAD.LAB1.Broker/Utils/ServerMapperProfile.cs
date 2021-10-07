using AutoMapper;
using GrpcAgent;
using PAD.LAB1.Broker.Models;

namespace PAD.LAB1.Broker.Utils
{
    public class ServerMapperProfile : Profile
    {
        public ServerMapperProfile()
        {
            CreateMap<RoomModel, Room>().ReverseMap();
        }
    }
}
