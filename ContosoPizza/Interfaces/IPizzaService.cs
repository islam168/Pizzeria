using ContosoPizza.ViewModel;
using ContosoPizza.ViewModels;

namespace ContosoPizza.Interface
{
    public interface IPizzaService
    {
        Task<List<PizzaViewModel>> GetAllPizzas();
        Task<PizzaViewModel?> GetPizza(int pizzaId);
        Task<ServiceResponse> CreatePizza(CreatePizzaViewModel pizza);
        Task<ServiceResponse> UpdatePizza(CreatePizzaViewModel pizza);
        Task<ServiceResponse> DeletePizza(int id);
    }
}
