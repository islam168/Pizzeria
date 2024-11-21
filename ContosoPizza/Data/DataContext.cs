using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Pizza> Pizzas { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pizza>()
                .HasIndex(e => e.Name)
                .IsUnique(); // Уникальное название пиццы.

            // Связь один к одному между Customer и CreditCard с каскадным удалением.
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.CreditCard)
                .WithOne(cc => cc.Customer)
                .HasForeignKey<CreditCard>(cc => cc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один к одному между Customer и Cart с каскадным удалением.
            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Customer)
                .WithOne(c => c.Cart)
                .HasForeignKey<Customer>(c => c.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один ко многим между Customer и Order с удалением типа restrict
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Customer)
                .WithMany(c => c.Orders)
                .HasForeignKey(o => o.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // Заказы не будут удалены при удалении клиента

            // Связь один ко многим между Customer и FeedBack с удалением типа SetNull
            modelBuilder.Entity<FeedBack>()
                .HasOne(f => f.Customer)
                .WithMany(c => c.FeedBacks)
                .HasForeignKey(f => f.CustomerId)
                .OnDelete(DeleteBehavior.SetNull); // При удалении клиента (customer) отзывы не удалены,
                                                   // поле customer становится null. 

            // Связь один ко многим между Order и FeedBack с каскадным удаленим
            modelBuilder.Entity<FeedBack>()
                .HasOne(f => f.Order)
                .WithMany(o => o.FeedBacks)
                .HasForeignKey(f => f.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один ко многим между Pizza и OrderItem с каскадным удалением
            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Pizza)
                .WithMany(p => p.OrderItems)
                .HasForeignKey(o => o.PizzaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь один ко многим между таблицами Cart и OrderItem с каскадным удалением
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Cart)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            // Связь многое ко многим между Order и Promotion
            modelBuilder.Entity<Order>()
                .HasMany(o => o.Promotions)
                .WithMany(p => p.Orders)
                .UsingEntity(j => j.ToTable("OrderPromotion")); // Промежуточная таблица.

            modelBuilder.Entity<Ingredient>()
                .HasMany(i => i.Pizzas)
                .WithMany(p => p.Ingredients)
                .UsingEntity(j => j.ToTable("PizzaIngredient")); // Промежуточная таблица.

            // Связь многое ко многим между User и Role
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);

        }
    }

}

