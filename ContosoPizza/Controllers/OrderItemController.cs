using ContosoPizza.Interface;
using ContosoPizza.Services;
using ContosoPizza.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpGet("customer/{customerId}/order-items")]
        public async Task<IActionResult> GetAllCustomerOrderItems(int customerId) 
        {
            var result = await _orderItemService.GetAllCustomerOrderItems(customerId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItem([FromBody] OrderItemViewModel orderItem)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var response = await _orderItemService.CreateOrderItem(orderItem);

            if (!response.Success)
            {
                return BadRequest(response.Message);  // Возвращаем ошибку с сообщением.
            }

            return Ok(response);  // Возвращаем успешное сообщение.
        }

        [HttpPut("{orderItemId}")]
        public async Task<IActionResult> UpdateOrderItem(int orderItemId, [FromBody] OrderItemViewModel orderItem)
        {
            if (orderItemId != orderItem.Id)
                return BadRequest("Order item ID mismatch.");

            var response = await _orderItemService.UpdateOrderItem(orderItem);

            if (!response.Success)
            {
                return BadRequest(response.Message);  // Возвращаем ошибку с сообщением
            }

            return Ok(response);  // Возвращаем успешное сообщение


        }
    }
}
