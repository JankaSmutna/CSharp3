namespace ToDoList.Test.UnitTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class ReadTests
{
    [Fact]
    public async Task Get_ReadWhenSomeItemsAvailable_ReturnsOk()
    {
        // Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        var listOfItems = new ToDoItem { Name = "Get method", Description = "Testing the Get/Read method - OK", IsCompleted = false };
        repository.Read().Returns([listOfItems]);

        // Act
        var result = await controller.Read();

        // Assert
        Assert.IsType<ActionResult<IEnumerable<ToDoItemGetResponseDto>>>(result);
        await repository.Received(1).Read();
    }

    [Fact]
    public async Task Get_ReadWhenNoItemAvailable_ReturnsNotFound()
    {
        //Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        repository.Read().Returns(new List<ToDoItem>());

        //Act
        var result = await controller.Read();

        // Assert
        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        await repository.Received(1).Read();
    }

    [Fact]
    public async Task Get_ReadUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        repository.When(r => r.Read())
                  .Do(static x => throw new Exception("Unexpected error."));

        // Act
        var result = await controller.Read();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        await repository.Received(1).Read();
    }
}
