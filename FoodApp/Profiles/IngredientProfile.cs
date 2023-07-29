using AutoMapper;

using FoodApp.Entities;
using FoodApp.Models;

namespace FoodApp.Profiles
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile() 
        {
            CreateMap<Ingredient, IngredientDto>()
                .ForMember(
                d => d.DishId,
                o => o.MapFrom(s => s.Dishes.First().Id));
        }
    }
}
