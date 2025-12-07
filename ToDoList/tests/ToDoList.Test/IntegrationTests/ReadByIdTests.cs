namespace ToDoList.Test.IntegrationTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class ReadByIdTests
{
    private const string DbPath = "../../../IntegrationTests/data/localdb_test.db";

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public async Task ReadById_ReturnsCorrectResult_WhenIdExists(int id)
    {
        // Arrange
        TestBase.CreateDatabase();
        await using var context = new ToDoItemsContext($"Data Source={DbPath}");
        await context.Database.EnsureCreatedAsync();

        // Naplnění db
        for (int i = 1; i <= 5; i++)
        {
            var toDoItem = new ToDoItem
            {
                ToDoItemId = i,
                Name = $"Název {i}",
                Description = $"Popis {i}",
                IsCompleted = false
            };

            await context.ToDoItems.AddAsync(toDoItem);
        }

        await context.SaveChangesAsync();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        // Act
        var result = await controller.ReadById(id);

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Result);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

        var item = Assert.IsType<ToDoItemGetResponseDto>(okResult.Value);
        Assert.Equal(id, item.ToDoItemId);
        Assert.Equal($"Název {id}", item.Name);
        Assert.Equal($"Popis {id}", item.Description);
        Assert.False(item.IsCompleted);

        // Cleanup
        TestBase.DeleteDatabase();
    }

    [Theory]
    [InlineData(999)]
    [InlineData(-1)]
    public async Task ReadById_ReturnsNotFound_WhenIdDoesNotExist(int id)
    {
        // Arrange
        TestBase.CreateDatabase();
        await using var context = new ToDoItemsContext($"Data Source={DbPath}");
        await context.Database.EnsureCreatedAsync();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);

        // Act
        var result = await controller.ReadById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);

        // Cleanup
        TestBase.DeleteDatabase();
    }
}
