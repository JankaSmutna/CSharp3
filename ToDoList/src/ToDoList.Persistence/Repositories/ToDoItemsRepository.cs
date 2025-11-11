namespace ToDoList.Persistence.Repositories;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsRepository : IRepository<ToDoItem>
{
    private readonly ToDoItemsContext context;

    public ToDoItemsRepository(ToDoItemsContext context)
    {
        this.context = context;
    }

    public void Create(ToDoItem item)
    {
        context.ToDoItems.Add(item);
        context.SaveChanges();
    }

    public IEnumerable<ToDoItem> Read()
    {
        return context.ToDoItems.AsNoTracking().ToList();
    }

    public ToDoItem? ReadById(int id)
    {
        return context.ToDoItems.AsNoTracking().FirstOrDefault(x => x.ToDoItemId == id);
    }

    public bool UpdateById(int id, ToDoItem updatedItem)
    {
        var existingItem = context.ToDoItems.FirstOrDefault(x => x.ToDoItemId == id);

        if (existingItem == null)
        {
            return false;
        }

        existingItem.Name = updatedItem.Name;
        existingItem.Description = updatedItem.Description;
        existingItem.IsCompleted = updatedItem.IsCompleted;

        context.SaveChanges();
        return true;
    }

    public bool DeleteById(int id)
    {
        var item = context.ToDoItems.FirstOrDefault(x => x.ToDoItemId == id);

        if (item == null)
        {
            return false;
        }

        context.ToDoItems.Remove(item);
        context.SaveChanges();
        return true;
    }
}
