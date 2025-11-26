namespace ToDoList.WebApi;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;

[Route("api/[controller]")] //(https://)localhost:5000/api/ToDoItems - hledáme na tomto zdroji
[ApiController] //třída podporující HTTP responses
public class ToDoItemsController : ControllerBase
{
    private readonly IRepositoryAsync<ToDoItem> repository;

    public ToDoItemsController(IRepositoryAsync<ToDoItem> repository)
    {
        this.repository = repository;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ToDoItemCreateRequestDto request)
    {
        var item = request.ToDomain();

        try
        {
            await repository.Create(item);

            return CreatedAtAction(nameof(ReadById), new { toDoItemsId = item.ToDoItemId }, ToDoItemGetResponseDto.FromDomain(item));
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ToDoItemGetResponseDto>>> Read()
    {
        try
        {
            var listOfItems = await repository.Read();
            var dtos = listOfItems.Select(ToDoItemGetResponseDto.FromDomain).ToList();

            if (dtos.Count == 0)
            {
                return NotFound(); //404
            }

            return Ok(dtos); //200
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpGet("{toDoItemsId:int}")]
    public async Task<ActionResult<ToDoItemGetResponseDto>> ReadById(int toDoItemsId)
    {
        try
        {
            var item = await repository.ReadById(toDoItemsId);

            if (item == null)
            {
                return NotFound(); //404
            }

            var dto = ToDoItemGetResponseDto.FromDomain(item);
            return Ok(dto); //200
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpPut("{toDoItemsId:int}")]
    public async Task<ActionResult> UpdateById(int toDoItemsId, [FromBody] ToDoItemUpdateRequestDto request)
    {
        var updatedItem = request.ToDomain();

        try
        {
            bool itemWasUpdated = await repository.UpdateById(toDoItemsId, updatedItem);

            if (!itemWasUpdated)
            {
                return NotFound(); //404
            }

            return NoContent(); //204
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }

    [HttpDelete("{toDoItemsId:int}")]
    public async Task<ActionResult> DeleteById(int toDoItemsId)
    {
        try
        {
            bool itemWasDeleted = await repository.DeleteById(toDoItemsId);

            if (!itemWasDeleted)
            {
                return NotFound(); //404
            }

            return NoContent(); //204
        }

        catch (Exception e)
        {
            return Problem(e.Message, null, StatusCodes.Status500InternalServerError); //500
        }
    }
}
