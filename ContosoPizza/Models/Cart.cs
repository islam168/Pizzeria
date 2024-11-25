using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPizza.Models
{
    // Корзина
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; } // Внешний ключ.
        public Customer Customer { get; set; } // Навигационное свойство.
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        [Column(TypeName = "decimal(8,2)")] // Указание типа и точности.
        public Decimal TotalAmount { get; set; } // Общая стоимость заказа. 
    }
}
