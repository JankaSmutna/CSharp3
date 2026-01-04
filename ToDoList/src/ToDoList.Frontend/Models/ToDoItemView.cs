namespace ToDoList.Frontend.Models;

using System.ComponentModel.DataAnnotations;

public class ToDoItemView
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is mandatory.")]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Description is mandatory.")]
    [StringLength(250)]
    public string Description { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public string? Category { get; set; }
}
