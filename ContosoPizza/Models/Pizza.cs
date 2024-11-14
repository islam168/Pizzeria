using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoPizza.Models;

public class Pizza
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    [Column(TypeName = "decimal(6,2)")] // Указание типа и точности.
    public decimal Price { get; set; }
    public string PizzaImage { get; set; }
    public bool IsAvailable { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Навигационное свойство.
    public ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
}