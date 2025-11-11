namespace ToDoList.Test.IntegrationTests;

using ToDoList.Domain.Models;

public class ReadTests
{
    private const string DbPath = "../../../IntegrationTests/data/localdb_test.db";

    [Fact]
    public void Read_ReturnsAllItems()
    {
        // Arrange
        TestBase.CreateDatabase();

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

        var todoItem2 = new ToDoItem
        {
            ToDoItemId = 2,
            Name = "Název 2",
            Description = "Popis 2",
            IsCompleted = true
        };

        context.ToDoItems.Add(todoItem1);
        context.ToDoItems.Add(todoItem2);
        context.SaveChanges();

        var controller = new ToDoItemsControllerTest(context);

        // Act
        var result = controller.Read();
        var value = result.GetValue();

        //Assert - ověření výsledku
        Assert.NotNull(value);

        var firstToDo = value.First();
        Assert.Equal(todoItem1.ToDoItemId, firstToDo.ToDoItemId);
        Assert.Equal(todoItem1.Name, firstToDo.Name);
        Assert.Equal(todoItem1.Description, firstToDo.Description);
        Assert.Equal(todoItem1.IsCompleted, firstToDo.IsCompleted);

        // Cleanup
        TestBase.DeleteDatabase();
    }
}
