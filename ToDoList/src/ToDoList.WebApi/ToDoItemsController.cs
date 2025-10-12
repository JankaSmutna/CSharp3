namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] //(https://)localhost:5000/api/ToDoItems - hledáme na tomto zdroji
[ApiController] //třída podporující HTTP responses
public class ToDoItemsController : ControllerBase
{
    private static readonly List<ToDoItem> items = []; //list vytvořený In-memory

    [HttpPost]
    public IActionResult Create([FromBody] ToDoItemCreateRequestDto request)  //používáme DTO - Data Transfer Object
    {
        try
        {
            var item = new ToDoItem
            {
                ToDoItemId = items.Count == 0 ? 1 : items.Max(x => x.ToDoItemId) + 1,
                Name = request.Name,
                Description = request.Description,
                IsCompleted = request.IsCompleted,
            };
            items.Add(item);
            return Ok(StatusCodes.Status201Created);
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }


        finally
        {
            Console.WriteLine("Metoda Create proběhla.");
        }
    }

    [HttpGet]
    public IActionResult Read() //api/ToDoItems GET
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

        finally
        {
            Console.WriteLine("Metoda Get proběhla.");
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

        finally
        {
            Console.WriteLine("Metoda GetById proběhla.");
        }
    }

    /*
    [HttpPut("{ToDoItemsId:int}")]
     public IActionResult UpdateById(int ToDoItemsId, ToDoItemUpdateRequestDto request)
     {
         try
         {
             throw new Exception("Něco se pravdu nepovedlo.");
         }

         catch (Exception ex)
         {
             return Problem(ex.Message, null, StatusCodes.Status500InternalServerError); //500
         }
     }

     [HttpDelete("{ToDoItemsId:int}")]
     public IActionResult DeleteById(int ToDoItemsId)
     {
         return Ok();
     }
     */
}
