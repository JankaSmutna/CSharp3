namespace ToDoList.Test.IntegrationTests;

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class UpdateByIdTests
{
    private const string DbPath = "../../../IntegrationTests/data/localdb_test.db";

    [Fact]
    public async Task UpdateById_ReturnsCorrectResult_WhenItemIsUpdated()
    {
        // Arrange
        TestBase.CreateDatabase();
        using var context = new ToDoItemsContext($"Data Source={DbPath}");
        await context.Database.EnsureCreatedAsync();

        // Naplnění db
        var toDoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Název",
            Description = "Popis",
            IsCompleted = false
        };

        context.ToDoItems.Add(toDoItem);
        await context.SaveChangesAsync();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        var updateDto = new ToDoItemUpdateRequestDto("Nový název", "Nový popis", true);

        // Act
        var result = await controller.UpdateById(1, updateDto);

        // Assert
        var noContentResult = Assert.IsType<NoContentResult>(result);
        Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);

        var updatedItem = context.ToDoItems.Find(1);
        Assert.NotNull(updatedItem);
        Assert.Equal("Nový název", updatedItem.Name);
        Assert.Equal("Nový popis", updatedItem.Description);
        Assert.True(updatedItem.IsCompleted);

        // Cleanup
        TestBase.DeleteDatabase();
    }
}
