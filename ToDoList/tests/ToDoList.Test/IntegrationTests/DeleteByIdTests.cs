namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using ToDoList.Domain.Models;

public class DeletyByIdTests
{
    private const string DbPath = "../../../IntegrationTests/data/localdb_test.db";

    [Fact]
    public void DeleteById_ReturnsNoContent_WhenItemExists()
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
        var todoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Název 1",
            Description = "Popis 1",
            IsCompleted = false
        };

        context.ToDoItems.Add(todoItem1);
        context.SaveChanges();

        var controller = new ToDoItemsControllerTest(context);

        // Act
        var result = controller.DeleteById(1);

        // Assert - vrací ok status 204
        Assert.IsType<NoContentResult>(result);

        // Cleanup
        SqliteConnection.ClearAllPools();

        if (File.Exists(DbPath))
        {
            File.Delete(DbPath);
        }
    }

    [Fact]
    public void DeleteById_RemovesItemCorrectly()
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
        controller.DeleteById(2);
        var result = controller.ReadById(2);

        // Assert - vrací status 404, pokud se snaží najít položku s Id = 2, která byla vymazána
        Assert.IsType<NotFoundResult>(result);

        // Cleanup
        SqliteConnection.ClearAllPools();

        if (File.Exists(DbPath))
        {
            File.Delete(DbPath);
        }
    }
}
