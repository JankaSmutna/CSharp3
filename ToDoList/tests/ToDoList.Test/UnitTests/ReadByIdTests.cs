namespace ToDoList.Test.UnitTests;

using System.Threading.Tasks;
using Xunit;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
using Microsoft.AspNetCore.Http;

public class ReadByIdTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]

    public async Task Get_ReadByIdWhenSomeItemAvailable_ReturnsOk(int id)
    {
        // Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        var toDoItem = new ToDoItem
        {
            ToDoItemId = id,
            Name = $"Get method {id}",
            Description = $"Testing the Get/ReadById method - OK {id}",
            IsCompleted = false
        };

        repository.ReadById(id).Returns(Task.FromResult<ToDoItem?>(toDoItem));

        // Act
        var result = await controller.ReadById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var item = Assert.IsType<ToDoItemGetResponseDto>(okResult.Value);

        Assert.Equal(id, item.ToDoItemId);
        Assert.Equal($"Get method {id}", item.Name);
        Assert.Equal($"Testing the Get/ReadById method - OK {id}", item.Description);
        Assert.False(item.IsCompleted);

        await repository.Received(1).ReadById(id);
    }

    [Theory]
    [InlineData(999)]
    [InlineData(-1)]
    public async Task Get_ReadByIdWhenItemIsNull_ReturnsNotFound(int id)
    {
        // Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        repository.ReadById(id).Returns(Task.FromResult<ToDoItem?>(null));

        // Act
        var result = controller.ReadById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
        await repository.Received(1).ReadById(id);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(42)]
    public async Task Get_ReadByIdUnhandledException_ReturnsInternalServerError(int id)
    {
        // Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        repository.When(r => r.ReadById(id))
                  .Do(x => throw new Exception("Unexpected error"));

        // Act
        var result = await controller.ReadById(id);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        await repository.Received(1).ReadById(id);
    }
}
