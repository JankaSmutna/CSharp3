namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class CreateTests
{
    [Fact]
    public void Post_CreateValidRequest_ReturnsCreatedAtAction()
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        var dto = new ToDoItemCreateRequestDto("Post method", "Testing the Post/Create method - CreatedAtAction", false);

        // Act
        var result = controller.Create(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);

        var responseDto = Assert.IsType<ToDoItemGetResponseDto>(createdResult.Value);
        Assert.Equal(dto.Name, responseDto.Name);
        Assert.Equal(dto.Description, responseDto.Description);
        Assert.Equal(dto.IsCompleted, responseDto.IsCompleted);
        repository.Received(1).Create(Arg.Any<ToDoItem>());
    }

    [Fact]
    public void Post_CreateUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        var dto = new ToDoItemCreateRequestDto("Post method", "Testing the Post/Create method - InternalServerError", false);

        repository.When(r => r.Create(Arg.Any<ToDoItem>()))
                  .Do(x => throw new Exception("Unexpected error"));

        // Act
        var result = controller.Create(dto);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        repository.Received(1).Create(Arg.Any<ToDoItem>());
    }
}
