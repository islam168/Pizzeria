namespace ContosoPizza.ViewModel
{
    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public int Quantity { get; set; } // Количество пиццы.
        public decimal Price { get; set; } // Цена за одну пиццу * количество.
        public int CartId { get; set; }
        public int PizzaId { get; set; }
    }

    public class CreateOrderItemViewModel
    {
        public int Quantity { get; set; } 

        public int PizzaId { get; set; }
    }
}
