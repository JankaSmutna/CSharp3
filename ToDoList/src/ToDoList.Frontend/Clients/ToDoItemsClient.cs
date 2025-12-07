namespace ToDoList.Frontend.Clients;

using ToDoList.Frontend.Models;
using ToDoList.Domain.DTOs;


public class ToDoItemsClient : IToDoItemsClient
{
    private readonly HttpClient httpClient;

    public ToDoItemsClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var toDoItemViews = new List<ToDoItemView>();
        var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems") ?? [];

        toDoItemViews = response.Select(dto => new ToDoItemView(dto.ToDoItemId, dto.Name, dto.Description, dto.IsCompleted)).ToList();

        return toDoItemViews;
    }
}
