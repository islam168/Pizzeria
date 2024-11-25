using ContosoPizza.Interface;
using ContosoPizza.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaService _pizzaService;
        public PizzaController(IPizzaService pizzaService)
        {
            _pizzaService = pizzaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPizzas()
        {
            var pizzas = await _pizzaService.GetAllPizzas();
            return Ok(pizzas);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetPizza(int id)
        {
            var pizza = await _pizzaService.GetPizza(id);
            return pizza != null ? Ok(pizza) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePizza([FromBody] CreatePizzaViewModel pizza)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _pizzaService.CreatePizza(pizza);

            if (!response.Success)
            {
                return BadRequest(response.Message);
            }

            return Ok(response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePizza(int id, [FromBody] UpdatePizzaViewModel pizza)
        {
            if (id != pizza.Id)
                return BadRequest(ModelState);

            var response = await _pizzaService.UpdatePizza(pizza);

            if (!response.Success && response.ErrorCode == 404)
                return NotFound(response.Message);

            else if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePizza(int id)
        {
            var response = await _pizzaService.DeletePizza(id);

            if (!response.Success && response.ErrorCode == 404)
                return NotFound(response.Message);

            else if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response);
        }
    }
}