using AutoMapper;
using AwesomeBackend.Shared.Models.Requests;
using AwesomeBackend.Shared.Models.Responses;
using Entities = AwesomeBackend.DataAccessLayer.Entities;

namespace AwesomeBackend.BusinessLayer.Mappers
{
    public class RatingMapperProfile : Profile
    {
        public RatingMapperProfile()
        {
            CreateMap<Entities.Rating, Rating>();

            CreateMap<RatingRequest, Entities.Rating>()
                .ForMember(dst => dst.Date, opt => opt.MapFrom(source => source.VisitedAt));
        }
    }
}
