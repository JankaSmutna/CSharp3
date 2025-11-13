namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class DeletyByIdTests
{
    private const string DbPath = "../../../IntegrationTests/data/localdb_test.db";

    [Fact]
    public void Delete_ValidId_ReturnsNoContent()
    {
        TestBase.CreateDatabase();
        using var context = new ToDoItemsContext($"Data Source={DbPath}");
        context.Database.EnsureCreated();

        // Naplnění db
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "DeleteById method",
            Description = "Testing the DeleteById method - NoContentResult",
            IsCompleted = false
        };

        context.ToDoItems.Add(todoItem1);
        context.SaveChanges();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        // Act
        var result = controller.DeleteById(1);

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Cleanup
        TestBase.DeleteDatabase();
    }

    [Fact]
    public void Delete_InvalidId_ReturnsNotFound()
    {
        // Arrange
        TestBase.CreateDatabase();
        using var context = new ToDoItemsContext($"Data Source={DbPath}");
        context.Database.EnsureCreated();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        // Act
        int invalidId = -1;
        var result = controller.DeleteById(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);

        // Cleanup
        TestBase.DeleteDatabase();
    }
}
