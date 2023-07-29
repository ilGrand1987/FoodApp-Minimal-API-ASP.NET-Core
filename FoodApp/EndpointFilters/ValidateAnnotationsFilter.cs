using FoodApp.Models;

using MiniValidation;

namespace FoodApp.EndpointFilters
{
    public class ValidateAnnotationsFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(
            EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var dishForCreationDto = context.GetArgument<DishForCreationDto>(2);

            if(!MiniValidator.TryValidate(dishForCreationDto, out var validationErrors))
            {
                return TypedResults.ValidationProblem(validationErrors);
            }
            return await next(context);
        }
    }
}
