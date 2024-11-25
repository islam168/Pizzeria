using ContosoPizza.Models;

namespace ContosoPizza.ViewModels
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; } // Количество пиццы.
        public decimal Price { get; set; } // Цена за одну пиццу * количество.
        public int PizzaId { get; set; }
    }
    public class CartViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } // Внешний ключ.
        public Decimal TotalAmount { get; set; } // Общая стоимость заказа. 
        public ICollection<OrderItemDto> OrderItems { get; set; } = new List<OrderItemDto>();
    }
}
