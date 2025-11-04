namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class UpdateByIdTests
{
    private const string DbPath = "../../../IntegrationTests/data/localdb_test.db";

    [Fact]
    public void UpdateById_ReturnsCorrectResult_WhenItemIsUpdated()
    {
        // Arrange
        TestBase.CreateDatabase();

        var context = new ToDoItemsContextTest($"Data Source={DbPath}");
        context.Database.EnsureCreated();

        // 3. Naplnění db
        var todoItem = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Název",
            Description = "Popis",
            IsCompleted = false
        };

        context.ToDoItems.Add(todoItem);
        context.SaveChanges();

        var controller = new ToDoItemsControllerTest(context);
        var dto = new ToDoItemUpdateRequestDto("Nový název", "Nový popis", true);

        // Act
        controller.UpdateById(1, dto);

        var result = controller.ReadById(1) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var updatedItem = result.Value as ToDoItemGetResponseDto;

        Assert.NotNull(updatedItem);
        Assert.Equal(1, updatedItem.ToDoItemId);
        Assert.Equal("Nový název", updatedItem.Name);
        Assert.Equal("Nový popis", updatedItem.Description);
        Assert.True(updatedItem.IsCompleted);

        // Cleanup
        TestBase.DeleteDatabase();
    }
}
