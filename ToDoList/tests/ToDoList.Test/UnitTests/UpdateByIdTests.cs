namespace ToDoList.Test.UnitTests;

using Xunit;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class UpdateByIdTests
{
    [Fact]
    public void UpdateById_ReturnsNoContent_WhenItemIsUpdated()
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        var updatedItem = new ToDoItemUpdateRequestDto(
            Name: "Nový název",
            Description: "Nový popis",
            IsCompleted: true
        );

        // Simulace úspěšného updatování existující položky
        repository.UpdateById(1, Arg.Any<ToDoItem>()).Returns(true);

        // Act
        var result = controller.UpdateById(1, updatedItem);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void UpdateById_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        var updateDto = new ToDoItemUpdateRequestDto("Nový název", "Nový popis", true);

        // Simulace neexistující položky
        repository.UpdateById(Arg.Any<int>(), Arg.Any<ToDoItem>()).Returns(false);

        // Act
        var result = controller.UpdateById(999, updateDto);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
