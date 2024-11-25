using ContosoPizza.Data;
using ContosoPizza.ViewModel;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;
using ContosoPizza.Interface;
using ContosoPizza.Utilities.JWT;


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
            // Проверка, существует ли пользователь с данной электронной почтой.
            var existingCustomer = await _context.Customers.FirstOrDefaultAsync(c => c.Email == customer.Email.ToLower());

            if (existingCustomer != null)
                return ServiceResponse.FailureResponse("A customer with this email already exists.");

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(customer.Password);

            // Получение роли "customer".
            string role = "Customer";
            var customerRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == role);

            var newCustomer = new Customer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email.ToLower(),
                Address = customer.Address,
                Phone = customer.Phone,
                Password = passwordHash,
                RoleId = customerRole.Id,
            };

            _context.Customers.Add(newCustomer);
            await _context.SaveChangesAsync();

            // Создаем корзину для нового пользователя
            var newCart = new Cart
            {
                CustomerId = newCustomer.Id, // Указываем связь с пользователем
                TotalAmount = 0, // Изначально сумма заказа 0
                OrderItems = new List<OrderItem>() // Изначально корзина пуста
            };

            // Добавляем корзину в базу данных
            _context.Carts.Add(newCart);
            await _context.SaveChangesAsync();

            // Обновляем пользователя с привязкой к корзине
            newCustomer.CartId = newCart.Id;
            await _context.SaveChangesAsync();

            return ServiceResponse.SuccessResponse("Customer registered successfully.");
        }

        public async Task<ServiceResponse> LoginCustomer(LoginCustomerViewModel customer)
        {
            var existingCustomer = await _context.Customers.Include(c => c.Role)
                .FirstOrDefaultAsync(c => c.Email == customer.Email);

            if (existingCustomer == null)
                return ServiceResponse.FailureResponse("", 404);

            // Проверяем, совпадает ли введенный пароль с паролем в БД. 
            bool passwordMatches = BCrypt.Net.BCrypt.Verify(customer.Password, existingCustomer.Password);

            if (passwordMatches == false)
                return ServiceResponse.FailureResponse("Invalid password.");

            var token = _jwtProvider.GenerateToken(existingCustomer);

            return ServiceResponse.SuccessResponse(token);
        }
    }
}
