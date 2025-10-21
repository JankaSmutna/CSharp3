namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;

public class UpdateByIdTests
{
    [Fact]
    public void UpdateById_ReturnsCorrectResult_WhenItemIsUpdated()
    {
        // Arrange - vyčištění listu, vytvoření původního a updatovaného DTO
        var controller = new ToDoItemsController();
        controller.RemoveAllItemsFromStorage();

        var dto = new ToDoItemCreateRequestDto("Název", "Popis", false);
        controller.Create(dto);

        var updatedDto = new ToDoItemUpdateRequestDto("Nový název", "Nový popis", true);

        // Act
        controller.UpdateById(1, updatedDto);

        var result = controller.ReadById(1) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        var updatedItem = result.Value as ToDoItemGetResponseDto;

        Assert.NotNull(updatedItem);
        Assert.Equal(1, updatedItem.ToDoItemId);
        Assert.Equal("Nový název", updatedItem.Name);
        Assert.Equal("Nový popis", updatedItem.Description);
        Assert.True(updatedItem.IsCompleted);
    }

    [Fact]
    public void UpdateById_ReturnsOkStatus204NoContent_WhenItemExists()
    {
        // Arrange
        var controller = new ToDoItemsController();
        controller.RemoveAllItemsFromStorage();

        var dto = new ToDoItemCreateRequestDto("Název", "Popis", false);
        controller.Create(dto);

        var updatedDto = new ToDoItemUpdateRequestDto("Nový název", "Nový popis", true);

        // Act
        var result = controller.UpdateById(1, updatedDto);

        // Assert - vrací ok status 204
        Assert.IsType<NoContentResult>(result);
    }

    [Theory]
    [InlineData(999)]
    [InlineData(-1)]
    public void UpdateById_ReturnsStatus404NotFound_WhenItemDoesNotExist(int id)
    {
        // Arrange
        var controller = new ToDoItemsController();
        controller.RemoveAllItemsFromStorage();

        var updatedDto = new ToDoItemUpdateRequestDto("Nový název", "Nový popis", false);

        // Act
        var result = controller.UpdateById(id, updatedDto);

        // Assert - vrací status 404
        Assert.IsType<NotFoundResult>(result);
    }
}
