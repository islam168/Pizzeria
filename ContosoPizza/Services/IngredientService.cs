using ContosoPizza.Data;
using ContosoPizza.Interfaces;
using ContosoPizza.Models;
using ContosoPizza.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly DataContext _context;
        public IngredientService(DataContext context)
        {
            _context = context;
        }

        private IngredientViewModel MapToViewModel(Ingredient ingredient)
        {
            return new IngredientViewModel
            {
                Id = ingredient.Id,
                Name = ingredient.Name
            };
        }

        public async Task<List<IngredientViewModel>> GetAllIngedient()
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            return ingredients.Select(MapToViewModel).ToList();
        }

        public async Task<IngredientViewModel?> GetIngredient(int Id)
        {
            var ingredient = await _context.Ingredients.FindAsync(Id);
            return ingredient != null ? MapToViewModel(ingredient) : null;
        }


        public async Task<ServiceResponse> CreateIngredient(IngredientViewModel ingredient)
        {
            if (await _context.Ingredients.AnyAsync(i => i.Name == ingredient.Name))
                return ServiceResponse.FailureResponse("An ingredient with this already exists.");

            var newIngredient = new Ingredient
            {
                Name = ingredient.Name
            };

            _context.Ingredients.Add(newIngredient);
            await _context.SaveChangesAsync();

            return ServiceResponse.SuccessResponse("Ingredient created successfully.", MapToViewModel(newIngredient));
        }

        public async Task<ServiceResponse> DeleteIngredient(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
                return ServiceResponse.FailureResponse("Ingredient not found", 404);

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();

            return ServiceResponse.SuccessResponse("Ingredient deleted successfully.");
        }

        public async Task<ServiceResponse> UpdateIngredient(IngredientViewModel ingredient)
        {
            var existingIngredient = await _context.Ingredients.FindAsync(ingredient.Id);
            if (existingIngredient == null)
                return ServiceResponse.FailureResponse("Ingredient not found.", 404);

            _context.Entry(existingIngredient).CurrentValues.SetValues(ingredient);
            await _context.SaveChangesAsync();

            return ServiceResponse.SuccessResponse("Ingredient updated successfully.");
        }
    }
}
