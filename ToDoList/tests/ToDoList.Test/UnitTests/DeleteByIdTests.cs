namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class DeleteByIdTests
{
    [Fact]
    public void DeleteById_ReturnsNoContent_WhenItemIsDeleted()
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        // Simulace, že došlo ke smazání existující položky
        repository.DeleteById(1).Returns(true);

        // Act
        var result = controller.DeleteById(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeleteById_ReturnsNotFound_WhenItemDoesNotExist()
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repository);

        // Mock repository vrací false → položka neexistuje
        repository.DeleteById(Arg.Any<int>()).Returns(false);

        // Act
        var result = controller.DeleteById(999);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
