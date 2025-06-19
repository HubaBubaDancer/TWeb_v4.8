using AutoMapper;
using TWeb48.Models;

namespace TWeb48
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CarRequest, Car>();
            CreateMap<UpdateRequest, User>();
        }
    }
}