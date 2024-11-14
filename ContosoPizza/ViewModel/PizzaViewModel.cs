namespace ContosoPizza.ViewModels
{
    public class PizzaViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PizzaImage { get; set; }
        public bool IsAvailable { get; set; }
        public List<string> IngredientNames { get; set; } = new List<string>();

    }
}