using ContosoPizza.Data;
using ContosoPizza.Interface;
using ContosoPizza.Models;
using ContosoPizza.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Services;

public class PizzaService : IPizzaService
{
    private readonly DataContext _context;
    public PizzaService(DataContext context)
    {
       _context = context;
    }

    private PizzaViewModel MapToViewModel(Pizza pizza)  // Преобразование модели Pizza в PizzaViewModel.
    {
        return new PizzaViewModel
        {
            Id = pizza.Id,
            Name = pizza.Name,
            Description = pizza.Description,
            Price = pizza.Price,
            PizzaImage = pizza.PizzaImage,
            IsAvailable = pizza.IsAvailable,
            IngredientNames = pizza.Ingredients.Select(i => i.Name).ToList()
        };
    }

    public async Task<List<PizzaViewModel>> GetAllPizzas()
    {
        // Загружаем пиццы вместе с их ингредиентами
        var pizzas = await _context.Pizzas
                                   .Include(p => p.Ingredients) // Явно указываем, что хотим загрузить ингредиенты
                                   .ToListAsync();
        return pizzas.Select(MapToViewModel).ToList();
    }

    public async Task<PizzaViewModel?> GetPizza(int pizzaId)
    {
        var pizza = await _context.Pizzas.Include(p => p.Ingredients)
            .FirstOrDefaultAsync(p => p.Id == pizzaId);

        // Используем тернарный оператор для проверки существования пиццы
        // Тернарный оператор (?:) состоит из трех частей: 
        // 1. Условие (pizza != null)
        // 2. Выражение, которое выполняется, если условие истинно (MapToViewModel(pizza))
        // 3. Выражение, которое выполняется, если условие ложно (null)

        // Тернарный оператор является сокращенной формой для if-else и имеет следующий синтаксис:
        // условие ? выражение_если_истинно : выражение_если_ложно.
        // В данном случае условие - это проверка на null для объекта pizza.
        return pizza != null ? MapToViewModel(pizza) : null;
        // Если пицца найдена, возвращаем соответствующую модель, 
        // если нет — возвращаем null.
    }

    public async Task<ServiceResponse> CreatePizza(CreatePizzaViewModel pizza)
    {
        // Проверяем, существует ли уже какая-либо пицца с таким же именем.
        if (await _context.Pizzas.AnyAsync(p => p.Name == pizza.Name))
            return ServiceResponse.FailureResponse("A pizza with this name already exists.");

        // Создаем объект Pizza из данных ViewModel
        var newPizza = new Pizza
        {
            Name = pizza.Name,
            Description = pizza.Description,
            Price = pizza.Price,
            PizzaImage = pizza.PizzaImage,
            IsAvailable = pizza.IsAvailable
        };

        // Получаем ингредиенты по их ID и добавляем в пиццу
        var ingredients = await _context.Ingredients
            .Where(i => pizza.IngredientIds.Contains(i.Id))
            .ToListAsync();

        // Contains(i.Id) — это метод, который проверяет, содержит ли коллекция
        // viewModel.IngredientIds значение i.Id.

        newPizza.Ingredients = ingredients;

        // Добавляем объект Pizza в контекст.
        _context.Pizzas.Add(newPizza);
        await _context.SaveChangesAsync();

        // Возвращаем успешный ответ
        return ServiceResponse.SuccessResponse("Pizza created successfully.");

    }

    public async Task<ServiceResponse> UpdatePizza(UpdatePizzaViewModel pizza)
    {   
        var existingPizza = await _context.Pizzas
            .Include(p => p.Ingredients) // Включение (Include) компонентов для обновления отношений между многими и многими
            .FirstOrDefaultAsync(p => p.Id == pizza.Id);

        if (existingPizza == null)
            return ServiceResponse.FailureResponse("Pizza not found.", 404);

        existingPizza.Name = pizza.Name;
        existingPizza.Description = pizza.Description;
        existingPizza.Price = pizza.Price;
        existingPizza.PizzaImage = pizza.PizzaImage;
        existingPizza.IsAvailable = pizza.IsAvailable;

        var newIngredients = await _context.Ingredients
            .Where(i => pizza.IngredientIds.Contains(i.Id))
            .ToListAsync();

        //foreach (var ingredient in newIngredients)
        //{
        //    existingPizza.Ingredients.Add(ingredient);
        //}

        existingPizza.Ingredients = newIngredients;

        await _context.SaveChangesAsync();

        return ServiceResponse.SuccessResponse("Pizza updated successfully.");
    }

    public async Task<ServiceResponse> DeletePizza(int id)
    {
        var pizza = await _context.Pizzas.FindAsync(id);
        if (pizza == null)
            return ServiceResponse.FailureResponse("Pizza not found.", 404);

        _context.Pizzas.Remove(pizza);
        await _context.SaveChangesAsync();

        return ServiceResponse.SuccessResponse("Pizza deleted successfully.");
    }
}