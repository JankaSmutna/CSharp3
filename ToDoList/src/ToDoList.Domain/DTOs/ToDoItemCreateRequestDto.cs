namespace ToDoList.Domain.DTOs;

public record ToDoItemCreateRequestDto(string Name, string Description, bool IsCompleted) //objekt typu record poslaný klientem v metodě Create
{

}

