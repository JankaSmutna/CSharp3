namespace ToDoList.Test;

using Microsoft.EntityFrameworkCore;
using ToDoList.Domain.Models;

public class ToDoItemsContextTest : DbContext
{
    private readonly string connectionString;

    public ToDoItemsContextTest(string connectionString = "Data Source=../../../IntegrationTests/data/localdb_test.db")
    {
        this.connectionString = connectionString;
    }

    public DbSet<ToDoItem> ToDoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connectionString);
    }
}
