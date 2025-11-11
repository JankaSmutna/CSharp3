namespace ToDoList.Test.UnitTests;

using Xunit;
using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class ReadByIdTests
{
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]

    public void ReadById_ReturnsCorrectResult_WhenIdExists(int id)
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        var toDoItem = new ToDoItem
        {
            ToDoItemId = id,
            Name = $"Název {id}",
            Description = $"Popis {id}",
            IsCompleted = false
        };

        repository.ReadById(id).Returns(toDoItem);

        // Act
        var result = controller.ReadById(id);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var item = Assert.IsType<ToDoItemGetResponseDto>(okResult.Value);

        Assert.Equal(id, item.ToDoItemId);
        Assert.Equal($"Název {id}", item.Name);
        Assert.Equal($"Popis {id}", item.Description);
        Assert.False(item.IsCompleted);
    }

    [Theory]
    [InlineData(999)]
    [InlineData(-1)]
    public void ReadById_ReturnsNotFound_WhenIdDoesNotExist(int id)
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        repository.ReadById(id).Returns((ToDoItem?)null);

        // Act
        var result = controller.ReadById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}
