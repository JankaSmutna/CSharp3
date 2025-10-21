namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] //(https://)localhost:5000/api/ToDoItems - hledáme na tomto zdroji
[ApiController] //třída podporující HTTP responses
public class ToDoItemsController : ControllerBase
{
    private static readonly List<ToDoItem> items = //list vytvořený In-memory
    [
        new() {ToDoItemId = 1, Name = "Baby Care", Description = "Go for a walk", IsCompleted = false },
        new() {ToDoItemId = 2, Name = "Housework", Description = "Do laundry",    IsCompleted = false },
        new() {ToDoItemId = 3, Name = "Homework",  Description = "C#",            IsCompleted = false },
    ];

    [HttpPost]
    public IActionResult Create([FromBody] ToDoItemCreateRequestDto request)  //používáme DTO - Data Transfer Object
    {
        var item = request.ToDomain();

        try
        {
            item.ToDoItemId = items.Count == 0 ? 1 : items.Max(x => x.ToDoItemId) + 1;
            items.Add(item);
            return Ok(StatusCodes.Status201Created);
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read() //api/ToDoItems GET
    {
        if (items == null)
        {
            return NotFound(); //404
        }

        try
        {
            var listOfItems = items.Select(ToDoItemGetResponseDto.FromDomain).ToList();

            return Ok(listOfItems);
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpGet("{toDoItemsId:int}")]
    public IActionResult ReadById(int toDoItemsId) //api/ToDoItems/<id> GET
    {
        try
        {
            var item = items.FirstOrDefault(x => x.ToDoItemId == toDoItemsId);

            if (item == null)
            {
                return NotFound(); //404
            }

            var dto = ToDoItemGetResponseDto.FromDomain(item);
            return Ok(dto);
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpPut("{toDoItemsId:int}")]
    public IActionResult UpdateById(int toDoItemsId, [FromBody] ToDoItemUpdateRequestDto request) // api/ToDoItems/<id> PUT
    {
        var updatedItem = request.ToDomain();

        try
        {
            var item = items.FirstOrDefault(x => x.ToDoItemId == toDoItemsId);

            if (item == null)
            {
                return NotFound(); //404
            }

            item.Name = updatedItem.Name;
            item.Description = updatedItem.Description;
            item.IsCompleted = updatedItem.IsCompleted;

            return NoContent(); //204
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpDelete("{toDoItemsId:int}")]
    public IActionResult DeleteById(int toDoItemsId) // api/ToDoItems/<id> DELETE
    {
        try
        {
            var item = items.FirstOrDefault(x => x.ToDoItemId == toDoItemsId);

            if (item == null)
            {
                return NotFound(); //404
            }

            items.Remove(item);

            return NoContent(); //204
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    //metoda pro přidání prvků do seznamu
    public void AddItemToStorage(ToDoItem item) => items.Add(item);

    //metoda pro vymazání všech prvků ze seznamu
    public void RemoveAllItemsFromStorage() => items.Clear();
}
