namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

[Route("api/[controller]")] //(https://)localhost:5000/api/ToDoItems - hledáme na tomto zdroji
[ApiController] //třída podporující HTTP responses
public class ToDoItemsController : ControllerBase
{
    private static List<ToDoItem> items = []; //list vytvořený In-memory

    [HttpPost]
    public IActionResult Create(ToDoItemCreateRequestDto request) //používáme DTO - Data Transfer Object
    {
        return Ok();
    }

    [HttpGet]
    public IActionResult Read() //api/ToDoItems GET
    {
        return Ok();
    }

    [HttpGet("{ToDoItemsId:int}")]
    public IActionResult ReadById(int ToDoItemsId) //api/ToDoItems/<id> GET
    {
        return Ok();
    }

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
}
