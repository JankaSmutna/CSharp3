namespace ToDoList.Test.IntegrationTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class ReadTests
{
    private const string DbPath = "../../../IntegrationTests/data/localdb_test.db";

    [Fact]
    public async Task Read_ReturnsAllItems()
    {
        // Arrange
        TestBase.CreateDatabase();
        await using var context = new ToDoItemsContext($"Data Source={DbPath}");
        await context.Database.EnsureCreatedAsync();

        // Naplnění db
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Název 1",
            Description = "Popis 1",
            IsCompleted = false
        };

        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Název 2",
            Description = "Popis 2",
            IsCompleted = true
        };

        context.ToDoItems.Add(todoItem1);
        context.ToDoItems.Add(todoItem2);
        context.SaveChanges();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        // Act
        var result = await controller.Read();
        var value = result.GetValue();

        // Assert
        Assert.NotNull(value);
        Assert.IsType<ActionResult<IEnumerable<ToDoItemGetResponseDto>>>(result);

        var firstToDo = value.First();
        Assert.Equal(todoItem1.ToDoItemId, firstToDo.ToDoItemId);
        Assert.Equal(todoItem1.Name, firstToDo.Name);
        Assert.Equal(todoItem1.Description, firstToDo.Description);
        Assert.Equal(todoItem1.IsCompleted, firstToDo.IsCompleted);

        // Cleanup
        TestBase.DeleteDatabase();
    }
}
