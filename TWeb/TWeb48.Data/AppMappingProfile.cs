using AutoMapper;
using TWeb48.Api.Reference.Dtos;
using TWeb48.Data.Models;

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