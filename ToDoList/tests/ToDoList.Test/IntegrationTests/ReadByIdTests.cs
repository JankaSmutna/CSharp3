namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;

public class ReadByIdTests
{
    private const string DbPath = "../../../IntegrationTests/data/localdb_test.db";

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void ReadById_ReturnsCorrectResult_WhenIdExists(int id)
    {
        // Arrange
        // 1.  Vytvoření db
        string? directory = Path.GetDirectoryName(DbPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory!);
        }

        // 2. Případné vyčištění předchozí db
        if (File.Exists(DbPath))
        {
            File.Delete(DbPath);
        }

        var context = new ToDoItemsContextTest($"Data Source={DbPath}");
        context.Database.EnsureCreated();

        // 3. Naplnění db
        for (int i = 1; i <= 5; i++)
        {
            var toDoItem = new ToDoItem
            {
                ToDoItemId = i,
                Name = $"Název {i}",
                Description = $"Popis {i}",
                IsCompleted = false
            };

            context.ToDoItems.Add(toDoItem);
        }

        context.SaveChanges();

        var controller = new ToDoItemsControllerTest(context);

        // Act
        var result = controller.ReadById(id) as OkObjectResult;

        // Assert
        Assert.NotNull(result);

        var item = result.Value as ToDoItemGetResponseDto;

        Assert.Equal(id, item.ToDoItemId);
        Assert.Equal($"Název {id}", item.Name);
        Assert.Equal($"Popis {id}", item.Description);
        Assert.False(item.IsCompleted);

        // Cleanup
        SqliteConnection.ClearAllPools();

        if (File.Exists(DbPath))
        {
            File.Delete(DbPath);
        }
    }

    [Theory]
    [InlineData(999)]
    [InlineData(-1)]
    public void ReadById_ReturnsNotFound_WhenIdDoesNotExist(int id)
    {
        // Arrange
        // 1.  Vytvoření db
        string? directory = Path.GetDirectoryName(DbPath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory!);
        }

        // 2. Případné vyčištění předchozí db
        if (File.Exists(DbPath))
        {
            File.Delete(DbPath);
        }

        var context = new ToDoItemsContextTest($"Data Source={DbPath}");
        context.Database.EnsureCreated();

        var controller = new ToDoItemsControllerTest(context);

        // Act
        var result = controller.ReadById(id);

        // Assert - vrací status 404
        Assert.IsType<NotFoundResult>(result);

        // Cleanup
        SqliteConnection.ClearAllPools();

        if (File.Exists(DbPath))
        {
            File.Delete(DbPath);
        }
    }
}
