namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record ToDoItemGetResponseDto(int ToDoItemId, string Name, string Description, bool IsCompleted)
{
    public static ToDoItemGetResponseDto FromDomain(ToDoItem item) => new(item.ToDoItemId, item.Name, item.Description, item.IsCompleted);
}
