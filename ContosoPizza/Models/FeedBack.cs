namespace ContosoPizza.Models
{
    public class FeedBack
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateOnly FeedBackDate { get; set; }

        public int? CustomerId { get; set; } // Внешний ключ. Знак "?" - допускает значение Null в CustomerId.
                                             // Это сделано для связи между таблицами с типом удаления SetNull
        public Customer? Customer { get; set; } // Навигационное свойство.
        public int OrderId { get; set; } // Внешний ключ.
        public Order Order { get; set; } // Навигационное свойство.
    }
}
