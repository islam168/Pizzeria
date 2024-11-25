using ContosoPizza.Interfaces;
using ContosoPizza.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;
        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIngredient()
        {
            var ingredient = await _ingredientService.GetAllIngedient();
            return Ok(ingredient);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetIngredient(int id)
        {
            var ingredient = await _ingredientService.GetIngredient(id);
            return ingredient != null ? Ok(ingredient) : NotFound();
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateIngredient([FromBody] IngredientViewModel ingredient)
        {
            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            var response = await _ingredientService.CreateIngredient(ingredient);

            if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateIngredient(int id, [FromBody] IngredientViewModel ingredient)
        {
            if (id != ingredient.Id)
                return BadRequest(ModelState);

            var response = await _ingredientService.UpdateIngredient(ingredient);

            if (!response.Success && response.ErrorCode == 404)
                return NotFound();

            else if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIngredient(int id)
        {
            var response = await _ingredientService.DeleteIngredient(id);

            if (!response.Success && response.ErrorCode == 404)
                return NotFound();

            else if (!response.Success)
                return BadRequest(response.Message);

            return Ok(response);
        }
    }
}
