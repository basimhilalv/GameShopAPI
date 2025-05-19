using AutoMapper;
using GameShopAPI.Models.UserModel;
using GameShopAPI.Models.UserModel.Dto;

namespace GameShopAPI.Mappings
{
    public class AppMappings : Profile
    {
        public AppMappings()
        {
            CreateMap<User, UserRegDto>().ReverseMap();
            CreateMap<User, UserRegResDto>().ReverseMap();
        }
    }
}
