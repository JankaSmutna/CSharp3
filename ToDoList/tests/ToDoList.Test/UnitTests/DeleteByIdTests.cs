namespace ToDoList.Test.UnitTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class DeleteByIdTests
{
    [Fact]
    public async Task Delete_DeleteByIdValidItemId_ReturnsNoContent()
    {
        // Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        repository.DeleteById(1).Returns(Task.FromResult(true));

        // Act
        var result = await controller.DeleteById(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await repository.Received(1).DeleteById(Arg.Any<int>());
    }

    [Fact]
    public async Task Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
    {
        // Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        repository.DeleteById(Arg.Any<int>()).Returns(Task.FromResult(false));

        // Act
        var result = await controller.DeleteById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await repository.Received(1).DeleteById(Arg.Any<int>());
    }

    [Fact]
    public async Task Delete_DeleteByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        repository.When(r => r.DeleteById(Arg.Any<int>()))
                  .Do(x => throw new Exception("Unexpected error"));

        // Act
        var result = await controller.DeleteById(1);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
        await repository.Received(1).DeleteById(Arg.Any<int>());
    }
}
