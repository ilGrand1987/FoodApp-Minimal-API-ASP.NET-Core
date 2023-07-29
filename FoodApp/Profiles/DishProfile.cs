using AutoMapper;

using FoodApp.Entities;
using FoodApp.Models;

namespace FoodApp.Profiles
{
    public class DishProfile : Profile
    {
        public DishProfile() 
        { 
            CreateMap<Dish, DishDto>();
            CreateMap<DishForCreationDto, Dish>();
            CreateMap<DishForUpdateDto, Dish>();
        }

    }
}
