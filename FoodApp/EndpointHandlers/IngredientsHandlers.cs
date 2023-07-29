using AutoMapper;

using FoodApp.DbContexts;
using FoodApp.Entities;
using FoodApp.Models;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FoodApp.EndpointHandlers
{
    public class IngredientsHandlers
    {
        public static async Task<Results<NotFound, Ok<IEnumerable<IngredientDto>>>>
            GetIngredientsAsync(
            DishesDbContext dishesDbContext,
            IMapper mapper, Guid dishId)
        {
            var dishEntity = await dishesDbContext.Dishes
            .FirstOrDefaultAsync(d => d.Id == dishId);
            if (dishEntity == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(mapper.Map<IEnumerable<IngredientDto>>((await
                dishesDbContext.Dishes
                .Include(d => d.Ingredients)
                .FirstOrDefaultAsync(d => d.Id == dishId))?.Ingredients));
        }
    }
}
            