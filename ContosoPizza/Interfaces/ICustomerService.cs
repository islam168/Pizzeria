using ContosoPizza.ViewModel;

namespace ContosoPizza.Interface
{
    public interface ICustomerService
    {
        Task<ServiceResponse> RegisterCustomer(RegisterCustomerViewModel customer);

        Task<ServiceResponse> LoginCustomer(LoginCustomerViewModel customer);
    }
}
