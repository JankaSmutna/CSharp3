namespace ToDoList.Test;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;

public class CreateTests
{
    [Fact]
    public void Create_ReturnsStatus201Created_WhenItemIsValid()
    {
        // Arrange
        var controller = new ToDoItemsController();
        var dto = new ToDoItemCreateRequestDto("Název - test", "Popis - test", false);

        // Act
        var result = controller.Create(dto) as OkObjectResult;

        // Assert - pokud přidáme validní položku do ToDoList, vrátí se 201
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status201Created, result.Value);
    }

    /*[Fact]
    public void Create_ContainsSingleItemWithCorrectId_WhenListWasClearedThenOneItemAdded()
    {
        // Arrange
        var controller = new ToDoItemsController();
        controller.RemoveAllItemsFromStorage();
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
    }*/
}
