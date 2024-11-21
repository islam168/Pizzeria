using ContosoPizza.ViewModel;

namespace ContosoPizza.Interface
{
    public interface IOrderItemService
    {
        Task<List<OrderItemViewModel>> GetAllCustomerOrderItems(int customerId);
        Task<OrderItemViewModel?> GetOrderItem(int orderItemId);
        Task<ServiceResponse> CreateOrderItem(CreateOrderItemViewModel orderItem, HttpRequest request);
        Task<ServiceResponse> UpdateOrderItem(OrderItemViewModel orderItem);
    }
}
