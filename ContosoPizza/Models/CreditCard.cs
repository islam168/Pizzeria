namespace ContosoPizza.Models
{
    public class CreditCard
    {
        public int Id { get; set; }
        public int CreditCardNumber { get; set; }  // Добавить Хэширование.
        public int Month { get; set; }
        public int Year { get; set; }

        public int CustomerId { get; set; } // Внешний ключ.
        public Customer Customer { get; set; } // Навигационное свойство.
    }
}
