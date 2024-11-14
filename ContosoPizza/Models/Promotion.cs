namespace ContosoPizza.Models
{
    public class Promotion
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public float DiscountPercentage { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool IsActive { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>(); // Навигационное свойство.
    }
}
