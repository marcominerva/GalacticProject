using AutoMapper;
using AwesomeBackend.Shared.Models.Responses;
using System;
using System.Linq;
using Entities = AwesomeBackend.DataAccessLayer.Entities;

namespace AwesomeBackend.BusinessLayer.Mappers
{
    public class RestaurantMapperProfile : Profile
    {
        public RestaurantMapperProfile()
        {
            CreateMap<Entities.Restaurant, Restaurant>()
                .ForMember(dto => dto.RatingsCount,
                    opt => opt.MapFrom(source => source.Ratings != null ? source.Ratings.Count : 0))
                .ForMember(dto => dto.RatingScore,
                    opt => opt.MapFrom(source => source.Ratings != null && source.Ratings.Any()
                        ? Math.Round(source.Ratings.Select(r => r.Score).Average(), 2) : (double?)null));

            CreateMap<Entities.Address, Address>();
        }
    }
}
