using ContosoPizza.Services;
using ContosoPizza.ViewModel;

namespace ContosoPizza.Interface
{
    public interface ICustomerService
    {
        Task<ServiceResponse> RegisterCustomer(RegisterCustomerViewModel customer);
    }
}
