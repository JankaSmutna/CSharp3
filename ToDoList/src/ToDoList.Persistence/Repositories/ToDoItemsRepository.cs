namespace ToDoList.Persistence.Repositories;

using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsRepository : IRepositoryAsync<ToDoItem>
{
    private readonly ToDoItemsContext context;

    public ToDoItemsRepository(ToDoItemsContext context)
    {
        this.context = context;
    }

    public async Task Create(ToDoItem item)
    {
        await context.ToDoItems.AddAsync(item);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ToDoItem>> Read()
    {
        return await context.ToDoItems.AsNoTracking().ToListAsync();
    }

    public async Task<ToDoItem?> ReadById(int id)
    {
        return await context.ToDoItems.AsNoTracking().FirstOrDefaultAsync(x => x.ToDoItemId == id);
    }

    public async Task<bool> UpdateById(int id, ToDoItem updatedItem)
    {
        var existingItem = await context.ToDoItems.FirstOrDefaultAsync(x => x.ToDoItemId == id);

        if (existingItem == null)
        {
            return false;
        }

        existingItem.Name = updatedItem.Name;
        existingItem.Description = updatedItem.Description;
        existingItem.IsCompleted = updatedItem.IsCompleted;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteById(int id)
    {
        var item = await context.ToDoItems.FirstOrDefaultAsync(x => x.ToDoItemId == id);

        if (item == null)
        {
            return false;
        }

        context.ToDoItems.Remove(item);
        await context.SaveChangesAsync();
        return true;
    }
}
