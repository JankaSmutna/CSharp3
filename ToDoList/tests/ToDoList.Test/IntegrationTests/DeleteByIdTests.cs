namespace ToDoList.Test.IntegrationTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class DeletyByIdTests
{
    private const string DbPath = "../../../IntegrationTests/data/localdb_test.db";

    [Fact]
    public async Task Delete_ValidId_ReturnsNoContent()
    {
        TestBase.CreateDatabase();
        await using var context = new ToDoItemsContext($"Data Source={DbPath}");
        await context.Database.EnsureCreatedAsync();

        // Naplnění db
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "DeleteById method",
            Description = "Testing the DeleteById method - NoContentResult",
            IsCompleted = false
        };

        await context.ToDoItems.AddAsync(todoItem1);
        await context.SaveChangesAsync();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        // Act
        var result = await controller.DeleteById(1);

        // Assert
        Assert.IsType<NoContentResult>(result);

        // Cleanup
        TestBase.DeleteDatabase();
    }

    [Fact]
    public async Task Delete_InvalidId_ReturnsNotFound()
    {
        // Arrange
        TestBase.CreateDatabase();
        await using var context = new ToDoItemsContext($"Data Source={DbPath}");
        await context.Database.EnsureCreatedAsync();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        // Act
        int invalidId = -1;
        var result = await controller.DeleteById(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);

        // Cleanup
        TestBase.DeleteDatabase();
    }
}
