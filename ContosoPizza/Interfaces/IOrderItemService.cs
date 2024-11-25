using ContosoPizza.ViewModel;

namespace ContosoPizza.Interface
{
    public interface IOrderItemService
    {
        Task<List<OrderItemViewModel>> GetAllCustomerOrderItems(int customerId);
        Task<OrderItemViewModel?> GetOrderItem(int orderItemId);
        Task<ServiceResponse> CreateOrderItem(CreateOrderItemViewModel orderItem, int customerId);
        Task<ServiceResponse> UpdateOrderItem(OrderItemViewModel orderItem);
        Task<ServiceResponse> DeleteOrderItem(int orderItemId);
    }
}
