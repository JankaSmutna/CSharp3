namespace ToDoList.Frontend.Clients;

using ToDoList.Domain.DTOs;
using ToDoList.Frontend.Models;

public class ToDoItemsClient : IToDoItemsClient
{
    private readonly HttpClient httpClient;

    public ToDoItemsClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<List<ToDoItemView>> ReadItemsAsync()
    {
        var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems");

        if (response is null)
            return new List<ToDoItemView>();

        return response.Select(dto => new ToDoItemView
        {
            Id = dto.ToDoItemId,
            Name = dto.Name,
            Description = dto.Description,
            IsCompleted = dto.IsCompleted
        }).ToList();
    }

    public async Task<ToDoItemView?> ReadItemByIdAsync(int itemId)
    {
        var response = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{itemId}");

        if (response == null)
        {
            return null;
        }

        return new ToDoItemView
        {
            Id = response.ToDoItemId,
            Name = response.Name,
            Description = response.Description,
            IsCompleted = response.IsCompleted
        };
    }

    public async Task UpdateItemAsync(ToDoItemView item)
    {
        // try {}
        var itemRequest = new ToDoItemUpdateRequestDto(item.Name, item.Description, item.IsCompleted);
        var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id}", itemRequest);
    }

    public async Task DeleteItemAsync(int itemId)
    {
        var response = await httpClient.DeleteAsync($"api/ToDoItems/{itemId}");
    }
}
