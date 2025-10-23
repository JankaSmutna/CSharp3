namespace ToDoList.Domain.Models;

using System.ComponentModel.DataAnnotations;

public class ToDoItem
{
    [Key] // specifikování klíče
    public int ToDoItemId { get; set; } // Ef core hledá nejpravděpodobnější <id> nebo <nameId>
    [Length(1, 50)]
    public string Name { get; set; } = null!;
    [StringLength(250)]
    public string Description { get; set; } = null!;
    public bool IsCompleted { get; set; }
}
