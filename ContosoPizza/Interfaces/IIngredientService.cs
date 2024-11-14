using ContosoPizza.ViewModel;

namespace ContosoPizza.Interfaces
{
    public interface IIngredientService
    {
        Task<List<IngredientViewModel>> GetAllIngedient();
        Task<IngredientViewModel?> GetIngredient(int Id);
        Task<ServiceResponse> CreateIngredient(IngredientViewModel ingredient);
        Task<ServiceResponse> UpdateIngredient(IngredientViewModel ingredient);
        Task<ServiceResponse> DeleteIngredient(int Id);
    }
}
