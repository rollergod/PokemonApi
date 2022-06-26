using AutoMapper;
using Project_WebApi.Dto;
using Project_WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_WebApi.Helper
{
    /// <summary>
    /// Класс для Mapper. 
    /// </summary>
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Сопоставляем класс из которого будут приходить данные и класс в который будут отправляться данные
            CreateMap<Pokemon, PokemonDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Country, CountryDto>();
            CreateMap<Owner, OwnerDto>();
            CreateMap<Review, ReviewDto>();
            CreateMap<Reviewer, ReviewerDto>();

        }
    }
}
