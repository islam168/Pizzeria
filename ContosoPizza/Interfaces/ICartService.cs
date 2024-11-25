using ContosoPizza.ViewModels;

namespace ContosoPizza.Interfaces
{
    public interface ICartService
    {
        Task<CartViewModel> GetCustomerCart(int customerId);
    }
}
