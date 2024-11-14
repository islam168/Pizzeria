using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPizza.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDateTime { get; set; }
        [Column(TypeName = "decimal(8,2)")] // Указание типа и точности
        public Decimal TotalAmount { get; set; }
        public int CustomerId { get; set; } // Внешний ключ.
        public Customer Customer { get; set; } // Навигационное свойcтво.
        public ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>(); // Навигационное свойство.
        public ICollection <Promotion> Promotions { get; set; } = new List <Promotion>(); // Навигационное свойство.
    }
}
