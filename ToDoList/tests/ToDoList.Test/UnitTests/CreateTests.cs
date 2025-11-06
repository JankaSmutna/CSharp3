namespace ToDoList.Test.UnitTests;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using ToDoList.Domain.DTOs;
using ToDoList.Domain.Models;
using ToDoList.Persistence.Repositories;
using ToDoList.WebApi;

public class CreateTests
{
    [Fact]
    public void Create_ReturnsStatus201Created_WhenItemIsValid()
    {
        // Arrange
        var repository = Substitute.For<IRepository<ToDoItem>>();
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
    }
}
