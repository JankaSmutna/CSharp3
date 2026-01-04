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

public class UpdateByIdTests
{
    [Fact]
    public async Task Put_UpdateByIdWhenItemUpdated_ReturnsNoContent()
    {
        // Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        var updateDto = new ToDoItemUpdateRequestDto("Put method", "Testing the Put/UpdateById method - NoContentResult", true, "");

        repository.UpdateById(1, Arg.Any<ToDoItem>()).Returns(true);

        // Act
        var result = await controller.UpdateById(1, updateDto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        await repository.Received(1).UpdateById(Arg.Any<int>(), Arg.Any<ToDoItem>());
    }

    [Fact]
    public async Task Put_UpdateByIdWhenIdNotFound_ReturnsNotFound()
    {
        // Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        var updateDto = new ToDoItemUpdateRequestDto("Put method", "Testing the Put/UpdateById method - NotFoundResult", true, "");

        repository.UpdateById(Arg.Any<int>(), Arg.Any<ToDoItem>()).Returns(Task.FromResult(false));

        // Act
        var result = await controller.UpdateById(999, updateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
        await repository.Received(1).UpdateById(Arg.Any<int>(), Arg.Any<ToDoItem>());
    }

    [Fact]
    public async Task Put_UpdateByIdUnhandledException_ReturnsInternalServerError()
    {
        // Arrange
        var repository = Substitute.For<IRepositoryAsync<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        var updateDto = new ToDoItemUpdateRequestDto("Put method", "Testing the Put/UpdateById method - InternalServerError", true, "");

        repository.When(r => r.UpdateById(Arg.Any<int>(), Arg.Any<ToDoItem>())).Do(x => throw new Exception("Unexpected error"));

        // Act
        var result = await controller.UpdateById(1, updateDto);

        // Assert
        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status500InternalServerError, statusCodeResult.StatusCode);

        await repository.Received(1).UpdateById(Arg.Any<int>(), Arg.Any<ToDoItem>());
    }
}
