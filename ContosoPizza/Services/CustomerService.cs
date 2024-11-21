using ContosoPizza.Data;
using ContosoPizza.ViewModel;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;
using ContosoPizza.Interface;
using ContosoPizza.Utilities.JWT;
using Microsoft.AspNetCore.Identity;


namespace ContosoPizza.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DataContext _context;
        private readonly IJWTProvider _jwtProvider;
        public CustomerService(DataContext context, IJWTProvider jwtProvider)
        {
            _context = context;
            _jwtProvider = jwtProvider;
        }

        public async Task<ServiceResponse> RegisterCustomer(RegisterCustomerViewModel customer)
        {
            var existingCustomer = await GetCustomerByEmailAsync(customer.Email);

            // Проверка, существует ли пользователь с данной электронной почтой.
            if (existingCustomer != null)
                return ServiceResponse.FailureResponse("A customer with this email already exists.");


            var passwordHash = BCrypt.Net.BCrypt.HashPassword(customer.Password);

            // Получение роли "customer".
            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Customer");

            var newCustomer = new Customer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Address = customer.Address,
                Phone = customer.Phone,
                Password = passwordHash,
                RoleId = customerRole.Id
            };

            await _context.Customers.AddAsync(newCustomer);
            await _context.SaveChangesAsync();

            // Создаем корзину для нового пользователя
            var newCart = new Cart
            {
                CustomerId = newCustomer.Id, // Указываем связь с пользователем
                TotalAmount = 0, // Изначально сумма заказа 0
                OrderItems = new List<OrderItem>() // Изначально корзина пуста
            };

            // Добавляем корзину в базу данных
            await _context.Carts.AddAsync(newCart);
            await _context.SaveChangesAsync();

            // Обновляем пользователя с привязкой к корзине
            newCustomer.CartId = newCart.Id;
            await _context.SaveChangesAsync();

            return ServiceResponse.SuccessResponse("Customer registered successfully.");
        }

        public async Task<ServiceResponse> LoginCustomer(LoginCustomerViewModel customer)
        {
            var existingCustomer = await GetCustomerByEmailAsync(customer.Email);

            if (existingCustomer == null)
                return ServiceResponse.FailureResponse("Customer not found.");

            // Проверяем, совпадает ли введенный пароль с паролем в БД. 
            bool passwordMatches = BCrypt.Net.BCrypt.Verify(customer.Password, existingCustomer.Password);

            if (passwordMatches == false)
                return ServiceResponse.FailureResponse("Invalid password.");

            var token = _jwtProvider.GenerateToken(existingCustomer);

            return ServiceResponse.SuccessResponse("Logined successfully", token);
        }

        // Проверка существует ли пользователь в БД.
        private async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers.Include(c => c.Role).FirstOrDefaultAsync(c => c.Email == email);
        }

    }
}
