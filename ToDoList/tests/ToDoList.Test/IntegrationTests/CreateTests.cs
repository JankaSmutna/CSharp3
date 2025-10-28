namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using ToDoList.Domain.DTOs;

public class CreateTests
{
    private const string DbPath = "../../../IntegrationTests/data/localdb_test.db";

    [Fact]
    public void Create_ReturnsStatus201Created_WhenItemIsValid()
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
        var dto = new ToDoItemCreateRequestDto("Název - test", "Popis - test", false);

        // Act
        var result = controller.Create(dto) as OkObjectResult;
        context.SaveChanges();

        // Assert - pokud přidáme validní položku do ToDoList, vrátí se 201
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status201Created, result.Value);

        // Cleanup
        SqliteConnection.ClearAllPools();

        if (File.Exists(DbPath))
        {
            File.Delete(DbPath);
        }
    }

    [Fact]
    public void Create_ContainsSingleItemWithCorrectId_WhenListWasClearedThenOneItemAdded()
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

        var dto = new ToDoItemCreateRequestDto("Název - test", "Popis - test", false);

        var controller = new ToDoItemsControllerTest(context);

        // Act
        controller.Create(dto);
        var result = controller.Read();
        var value = result.GetValue()!;

        // Assert - po smazání seznamu a přidání jedné položky má seznam právě jednu položku s Id = 1 a s očekávanými hodnotami dalších atributů
        Assert.Single(value);

        var firstToDo = value.First();

        Assert.Equal(1, firstToDo.ToDoItemId);
        Assert.Equal("Název - test", firstToDo.Name);
        Assert.Equal("Popis - test", firstToDo.Description);
        Assert.False(firstToDo.IsCompleted);

        // Cleanup
        SqliteConnection.ClearAllPools();

        if (File.Exists(DbPath))
        {
            File.Delete(DbPath);
        }
    }
}

