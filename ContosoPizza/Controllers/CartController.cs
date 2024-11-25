using ContosoPizza.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerCart(int customerId) 
        {
            var cart = await _cartService.GetCustomerCart(customerId);
            return Ok(cart);
        }
    }
}
