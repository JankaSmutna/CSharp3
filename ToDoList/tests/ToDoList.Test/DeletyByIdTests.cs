namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.WebApi;

public class DeletyByIdTests
{
    [Fact]
    public void DeleteById_ReturnsNoContent_WhenItemExists()
    {
        // Arrange - tady pracuji s původním statickým listem se 3 položkami
        var controller = new ToDoItemsController();

        // Act
        var result = controller.DeleteById(1);

        // Assert - vrací ok status 204
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeleteById_RemovesItemCorrectly()
    {
        // Arrange - tady pracuji s původním statickým listem se 3 položkami
        var controller = new ToDoItemsController();

        // Act
        controller.DeleteById(2);
        var result = controller.ReadById(2);

        // Assert - vrací status 404, pokud se snaží najít položku s Id = 2, která byla vymazána
        Assert.IsType<NotFoundResult>(result);
    }
}
