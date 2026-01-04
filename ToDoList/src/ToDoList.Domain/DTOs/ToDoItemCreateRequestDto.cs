namespace ToDoList.Domain.DTOs;

using ToDoList.Domain.Models;

public record ToDoItemCreateRequestDto(string Name, string Description, bool IsCompleted, string? Category) //objekt typu record poslaný klientem v metodě Create
{
    public ToDoItem ToDomain() => new() { Name = Name, Description = Description, IsCompleted = IsCompleted, Category = Category }; //metoda vrací nový doménový objekt typu ToDoItem konverzí z DTO pomocí ToDomain
}


