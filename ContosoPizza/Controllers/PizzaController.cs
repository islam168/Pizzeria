using ContosoPizza.Interface;
using ContosoPizza.Services;
using ContosoPizza.ViewModel;
using ContosoPizza.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

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
    public async Task<IActionResult> UpdatePizza(int id, [FromBody] CreatePizzaViewModel pizza)
    {
        if (id != pizza.Id)
            return BadRequest(ModelState);

        var response = await _pizzaService.UpdatePizza(pizza);

        if (!response.Success)
        {
            return NotFound(response.Message);  // Возвращаем ошибку с сообщением.
        }

        return Ok(response);  // Возвращаем успешное сообщение.

    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePizza(int id)
    {
        var response = await _pizzaService.DeletePizza(id);

        if (!response.Success)
        {
            return NotFound(response.Message);  // Возвращаем ошибку с сообщением.
        }

        return Ok(response);  // Возвращаем успешное сообщение.
    }
}