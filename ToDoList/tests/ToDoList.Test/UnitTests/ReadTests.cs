namespace ToDoList.Test.UnitTests;

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
    public void Read_ReturnsOkWithListOfAllItems_WhenRepositoryContainsData()
    {
        //Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        var items = new List<ToDoItem>
        {
            new ToDoItem { ToDoItemId = 1, Name = "Název 1", Description = "Popis 1", IsCompleted = false },
            new ToDoItem { ToDoItemId = 2, Name = "Název 2", Description = "Popis 2", IsCompleted = true }
        };

        repository.Read().Returns(items);

        //Act
        var result = controller.Read();

        //Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        var value = Assert.IsAssignableFrom<IEnumerable<ToDoItemGetResponseDto>>(okResult.Value);
        Assert.Equal(2, value.Count());

        var first = value.First();
        Assert.Equal(items[0].ToDoItemId, first.ToDoItemId);
        Assert.Equal(items[0].Name, first.Name);
        Assert.Equal(items[0].Description, first.Description);
        Assert.Equal(items[0].IsCompleted, first.IsCompleted);
    }

    [Fact]
    public void Read_ReturnsNotFound_WhenRepositoryContainsNoData()
    {
        //Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        repository.Read().Returns(new List<ToDoItem>());

        //Act
        var result = controller.Read();

        //Assert
        var notFound = Assert.IsType<NotFoundResult>(result.Result);
        Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
    }
}
