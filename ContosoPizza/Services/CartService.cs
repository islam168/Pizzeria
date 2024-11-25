using ContosoPizza.Data;
using ContosoPizza.Interfaces;
using ContosoPizza.Models;
using ContosoPizza.ViewModel;
using ContosoPizza.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services
{
    public class CartService : ICartService
    {
        private readonly DataContext _context;
        public CartService(DataContext context) 
        {
            _context = context;
        }
        public async Task<CartViewModel> GetCustomerCart(int customerId)
        {
            var cart = await _context.Carts.Include(c => c.OrderItems).FirstOrDefaultAsync(c => c.CustomerId == customerId);

            // Формируем CartViewModel
            var cartViewModel = new CartViewModel
            {
                Id = cart.Id,
                CustomerId = customerId,
                TotalAmount = cart.OrderItems.Sum(oi => oi.Price), // Получение общей суммы заказа.
                OrderItems = cart.OrderItems.Select(oi => new OrderItemDto
                {
                    Id = oi.Id,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    PizzaId = oi.PizzaId,
                }).ToList()


            };
            return cartViewModel;
        }
    }
}
