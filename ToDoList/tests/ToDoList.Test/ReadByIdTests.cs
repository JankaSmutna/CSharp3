namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi;

public class ReadByIdTests
{
    /* [Theory]
     [InlineData(1)]
     [InlineData(2)]
     [InlineData(3)]
     [InlineData(4)]
     [InlineData(5)]

     public void ReadById_ReturnsCorrectResult_WhenIdExists(int id)
     {
         // Arrange
         var controller = new ToDoItemsController();
         controller.RemoveAllItemsFromStorage();

         for (int i = 1; i <= 5; i++)
         {
             var request = new ToDoItemCreateRequestDto($"Název {i}", $"Popis {i}", false);
             controller.Create(request);
         }

         // Act
         var result = controller.ReadById(id) as OkObjectResult;

         // Assert - vrací správně všechny položky s nalezeným Id
         Assert.NotNull(result);
         var item = result.Value as ToDoItemGetResponseDto;

         Assert.NotNull(item);
         Assert.Equal(id, item.ToDoItemId);
         Assert.Equal($"Název {id}", item.Name);
         Assert.Equal($"Popis {id}", item.Description);
         Assert.False(item.IsCompleted);
     }*/

    [Theory]
    [InlineData(100)]
    [InlineData(-1)]
    public void ReadById_ReturnsNotFound_WhenIdDoesNotExist(int id)
    {
        // Arrange
        var controller = new ToDoItemsController();
        controller.RemoveAllItemsFromStorage();

        // Act
        var result = controller.ReadById(id);

        // Assert - vrací status 404
        Assert.IsType<NotFoundResult>(result);
    }
}

