namespace ToDoList.Test;

using ToDoList.Domain.Models;
using ToDoList.WebApi;

public class ReadTests
{
    /*[Fact]
    public void Read_ReturnsAllItems()
    {
        //Arrange - příprava testovacích dat
        var controller = new ToDoItemsController(); //instance zodpovědná za manipulaci s položkami úkolů
        controller.RemoveAllItemsFromStorage();

        var toDoItem1 = new ToDoItem
        {
            ToDoItemId = 1,
            Name = "Název1",
            Description = "Popis1",
            IsCompleted = false
        };

        var toDoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Název2",
            Description = "Popis2",
            IsCompleted = false
        };

        controller.AddItemToStorage(toDoItem1);
        controller.AddItemToStorage(toDoItem2);

        //Act - testování logiky
        var result = controller.Read();
        var value = result.GetValue();

        //Assert - ověření výsledku
        Assert.NotNull(value);

        var firstToDo = value.First();
        Assert.Equal(toDoItem1.ToDoItemId, firstToDo.ToDoItemId);
        Assert.Equal(toDoItem1.Name, firstToDo.Name);
        Assert.Equal(toDoItem1.Description, firstToDo.Description);
        Assert.Equal(toDoItem1.IsCompleted, firstToDo.IsCompleted);
    }*/
}
