namespace ContosoPizza.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; } // Добавить Хэширование.
        public int RoleId { get; set; } // Внешний ключ.
        public Role Role { get; set; }
    }
}
