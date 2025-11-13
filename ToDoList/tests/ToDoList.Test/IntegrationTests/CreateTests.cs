namespace ToDoList.Test.IntegrationTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.Persistence;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;
using ToDoList.Test;

public class CreateTests
{
    private const string DbPath = "../../../IntegrationTests/data/localdb_test.db";

    [Fact]
    public void Create_ValidRequest_ReturnsCreatedAtAction()
    {
        // Arrange
        TestBase.CreateDatabase();
        using var context = new ToDoItemsContext($"Data Source={DbPath}");
        context.Database.EnsureCreated();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        var dto = new ToDoItemCreateRequestDto("Název - test", "Popis - test", false);

        // Act
        var result = controller.Create(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);

        var responseDto = Assert.IsType<ToDoItemGetResponseDto>(createdResult.Value);
        Assert.Equal(dto.Name, responseDto.Name);
        Assert.Equal(dto.Description, responseDto.Description);
        Assert.Equal(dto.IsCompleted, responseDto.IsCompleted);

        // Cleanup
        TestBase.DeleteDatabase();
    }

    [Fact]
    public void Create_ContainsSingleItemWithCorrectId_WhenListWasClearedThenOneItemAdded()
    {
        // Arrange
        TestBase.CreateDatabase();
        using var context = new ToDoItemsContext($"Data Source={DbPath}");
        context.Database.EnsureCreated();

        var repository = new ToDoItemsRepository(context);
        var controller = new ToDoItemsController(repository);
        var dto = new ToDoItemCreateRequestDto("Název - test", "Popis - test", false);

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
        TestBase.DeleteDatabase();
    }
}

