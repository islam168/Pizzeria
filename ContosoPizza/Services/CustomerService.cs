using ContosoPizza.Data;
using ContosoPizza.ViewModel;
using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;
using ContosoPizza.Interface;


namespace ContosoPizza.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly DataContext _context;

        public CustomerService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse> RegisterCustomer(RegisterCustomerViewModel customer)
        {
            var existingCustomer = await _context.Customers
                        .FirstOrDefaultAsync(c => c.Email == customer.Email);

            // Проверка, существует ли пользователь с данной электронной почтой.
            if (existingCustomer != null)
                return ServiceResponse.FailureResponse("A customer with this email already exists.");


            var passwordHash = BCrypt.Net.BCrypt.HashPassword(customer.Password);

            var newCustomer = new Customer
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Address = customer.Address,
                Phone = customer.Phone,
                Password = passwordHash,
            };

            await _context.Customers.AddAsync(newCustomer);
            await _context.SaveChangesAsync();

            return ServiceResponse.SuccessResponse("Customer registered successfully.");
        }
    }
}
