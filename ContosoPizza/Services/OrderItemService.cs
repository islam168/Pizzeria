using ContosoPizza.Data;
using ContosoPizza.Interface;
using ContosoPizza.Models;
using ContosoPizza.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly DataContext _context;
        public OrderItemService(DataContext context)
        {
            _context = context;
        }

        private OrderItemViewModel MapToViewModel(OrderItem orderItem)
        {
            return new OrderItemViewModel
            {
                Id = orderItem.Id,
                Quantity = orderItem.Quantity,
                Price = orderItem.Price,
                CartId = orderItem.CartId,
                PizzaId = orderItem.PizzaId
            };
        }

        public async Task<List<OrderItemViewModel>> GetAllCustomerOrderItems(int customerId)
        {
            var orderItems = await _context.OrderItems
                .Where(p => p.CartId == customerId)
                .ToListAsync();
            return orderItems.Select(MapToViewModel).ToList();

        }

        public async Task<OrderItemViewModel?> GetOrderItem(int orderItemId)
        {
            var orderItem = await _context.OrderItems
                .FirstOrDefaultAsync(o => o.Id == orderItemId);
            return orderItem != null? MapToViewModel(orderItem) : null;
        }

        public async Task<ServiceResponse> CreateOrderItem(OrderItemViewModel orderItem)
        {
            var existingOrderItem = await _context.OrderItems
                .FirstOrDefaultAsync(o => o.CartId == orderItem.CartId && o.PizzaId == orderItem.PizzaId);

            if (existingOrderItem != null)
                return ServiceResponse.FailureResponse("Pizza is already in the cart.");

            // Create a new OrderItem based on the view model
            var newOrderItem = new OrderItem
            {
                Quantity = orderItem.Quantity,
                Price = _context.Pizzas
                .Where(p => p.Id == orderItem.PizzaId)
                .Select(p => p.Price)
                .FirstOrDefault() * orderItem.Quantity,
                CartId = orderItem.CartId,
                PizzaId = orderItem.PizzaId
            };

            _context.OrderItems.Add(newOrderItem);
            await _context.SaveChangesAsync();

            return ServiceResponse.SuccessResponse("Order Item created successfully.", MapToViewModel(newOrderItem));
        }

        public async Task<ServiceResponse> UpdateOrderItem(OrderItemViewModel orderItem)
        {
            var existingOrderItem = await _context.OrderItems
                .FirstOrDefaultAsync(c => c.Id == orderItem.Id);

            if (existingOrderItem == null)
                return ServiceResponse.FailureResponse("Order item not found.");

            existingOrderItem.Quantity = orderItem.Quantity;

            existingOrderItem.Price = _context.Pizzas
            .Where(p => p.Id == existingOrderItem.PizzaId)
            .Select(p => p.Price)
            .FirstOrDefault() * existingOrderItem.Quantity;

            await _context.SaveChangesAsync();

            return ServiceResponse.SuccessResponse("Order Item updated successfully.", MapToViewModel(existingOrderItem));
        }
    }
}
