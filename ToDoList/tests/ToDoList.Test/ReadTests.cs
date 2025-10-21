namespace ToDoList.Test;

using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class ReadTests
{
    [Fact]
    public void Read_ReturnsAllItems()
    {
        //Arrange - příprava testovacích dat
        var toDoItem4 = new ToDoItem
        {
            ToDoItemId = 4,
            Name = "Jmeno4",
            Description = "Popis4",
            IsCompleted = false
        };

        var toDoItem5 = new ToDoItem
        {
            ToDoItemId = 5,
            Name = "Jmeno5",
            Description = "Popis5",
            IsCompleted = false
        };

        var controller = new ToDoItemsController(); //instance zodpovědná za manipulaci s položkami úkolů
        controller.AddItemToStorage(toDoItem4);
        controller.AddItemToStorage(toDoItem5);

        //Act - testování logiky
        var result = controller.Read();
        var value = result.GetValue();

        //Assert - ověření výsledku
        Assert.NotNull(value);

        var fourthToDo = value.ElementAt(3);

        Assert.Equal(toDoItem4.ToDoItemId, fourthToDo.ToDoItemId);
        Assert.Equal(toDoItem4.Name, fourthToDo.Name);
        Assert.Equal(toDoItem4.Description, fourthToDo.Description);
        Assert.Equal(toDoItem4.IsCompleted, fourthToDo.IsCompleted);
    }

    [Fact]
    public void Read_ReturnsNotFound_WhenListIsEmpty()
    {
        // Arrange
        var controller = new ToDoItemsController();
        controller.RemoveAllItemsFromStorage();

        // Act
        var result = controller.Read();

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }
}
