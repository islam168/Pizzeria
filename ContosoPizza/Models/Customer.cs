namespace ContosoPizza.Models
{
    public class Customer : User
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public int CartId { get; set; } // Внешний ключ.
        public Cart Cart { get; set; } // Навигационное свойство.
        public CreditCard? CreditCard { get; set; } // Навигационное свойство.
        public ICollection<Order> Orders { get; set; } = new List<Order>(); // Навигационное свойство. Инициализация коллекции.
        public ICollection<FeedBack> FeedBacks { get; set; } = new List<FeedBack>(); // Навигационное свойство.
    }
}
