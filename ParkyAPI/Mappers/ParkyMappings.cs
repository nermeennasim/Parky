using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;

namespace ParkyAPI.Mappers
{
    public class ParkyMappings : Profile
    {
        public ParkyMappings()
        {
            CreateMap<NationalPark, NationalParkDtos>().ReverseMap();
            CreateMap<Trail, TrailDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap();
            CreateMap<Trail, TrailInsertDto>().ReverseMap();

            /* CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<Trail,TrailDto>().ReverseMap();
            CreateMap<Trail, TrailInsertDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap();*/
        }
    }
}
