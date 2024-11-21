using ContosoPizza.Interface;
using ContosoPizza.Services;
using ContosoPizza.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCustomerViewModel customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            var response = await _customerService.RegisterCustomer(customer);
            if (!response.Success)
            {
                return BadRequest(response.Message);  // Возвращаем ошибку с сообщением.
            }

            return Ok(response);  // Возвращаем успешное сообщение.
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCustomerViewModel customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _customerService.LoginCustomer(customer);
            if (!response.Success)
                return BadRequest(response.Message);

            HttpContext.Response.Cookies.Append("secretCookies", response.Data?.ToString());

            return Ok(response);
        }
    }
}
