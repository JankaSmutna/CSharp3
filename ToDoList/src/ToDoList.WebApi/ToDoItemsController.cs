namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;

[Route("api/[controller]")] //(https://)localhost:5000/api/ToDoItems - hledáme na tomto zdroji
[ApiController] //třída podporující HTTP responses
public class ToDoItemsController : ControllerBase
{
    private readonly ToDoItemsContext context;

    public ToDoItemsController(ToDoItemsContext context)
    {
        this.context = context;
    }

    [HttpPost]
    public ActionResult Create([FromBody] ToDoItemCreateRequestDto request)  //používáme DTO - Data Transfer Object
    {
        var item = request.ToDomain();

        try
        {
            context.ToDoItems.Add(item);
            context.SaveChanges();

            return CreatedAtAction(nameof(ReadById), new { toDoItemsId = item.ToDoItemId }, ToDoItemGetResponseDto.FromDomain(item));
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<ToDoItemGetResponseDto>> Read() //api/ToDoItems GET
    {
        if (context.ToDoItems == null)
        {
            return NotFound(); //404
        }

        try
        {
            var listOfItems = context.ToDoItems.AsNoTracking().Select(ToDoItemGetResponseDto.FromDomain).ToList();

            return Ok(listOfItems);
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpGet("{toDoItemsId:int}")]
    public ActionResult ReadById(int toDoItemsId) //api/ToDoItems/<id> GET
    {
        try
        {
            var item = context.ToDoItems.AsNoTracking().FirstOrDefault(x => x.ToDoItemId == toDoItemsId);

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
    public ActionResult UpdateById(int toDoItemsId, [FromBody] ToDoItemUpdateRequestDto request) // api/ToDoItems/<id> PUT
    {
        var updatedItem = request.ToDomain();

        try
        {
            var item = context.ToDoItems.FirstOrDefault(x => x.ToDoItemId == toDoItemsId);

            if (item == null)
            {
                return NotFound(); //404
            }

            item.Name = updatedItem.Name;
            item.Description = updatedItem.Description;
            item.IsCompleted = updatedItem.IsCompleted;

            context.SaveChanges();

            return NoContent(); //204
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpDelete("{toDoItemsId:int}")]
    public ActionResult DeleteById(int toDoItemsId) // api/ToDoItems/<id> DELETE
    {
        try
        {
            var item = context.ToDoItems.FirstOrDefault(x => x.ToDoItemId == toDoItemsId);

            if (item == null)
            {
                return NotFound(); //404
            }

            context.ToDoItems.Remove(item);
            context.SaveChanges();

            return NoContent(); //204
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    //metoda pro přidání prvků do seznamu
    public void AddItemToStorage(ToDoItem item) => context.ToDoItems.Add(item);
}
