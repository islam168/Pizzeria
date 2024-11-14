using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPizza.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; } // Количество пиццы.
        [Column(TypeName = "decimal(8,2)")] // Указание типа и точности.
        public decimal Price { get; set; } // Цена за одну пиццу * количество.

        public int CartId { get; set; } // Внешний ключ. CardId = CustomerId
        public Cart Cart { get; set; } // Навигационное свойство.

        public int PizzaId { get; set; } // Внешний ключ.
        public Pizza Pizza { get; set; } // Навигационное свойство.
    }
}
