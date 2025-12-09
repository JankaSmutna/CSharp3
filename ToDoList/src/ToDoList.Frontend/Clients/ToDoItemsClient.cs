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
        var toDoItemViews = new List<ToDoItemView>();
        try
        {
            var response = await httpClient.GetFromJsonAsync<List<ToDoItemGetResponseDto>>("api/ToDoItems");
            if (response is null)
            {
                Console.WriteLine($"GET request failed: No items to read.");
                return toDoItemViews;
            }
            toDoItemViews = response.Select(dto => new ToDoItemView
            {
                Id = dto.ToDoItemId,
                Name = dto.Name,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                Category = dto.Category
            }).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occured: {e.Message}");
        }

        return toDoItemViews;
    }

    public async Task<ToDoItemView?> ReadItemByIdAsync(int itemId)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<ToDoItemGetResponseDto>($"api/ToDoItems/{itemId}");
            if (response is null)
            {
                Console.WriteLine($"GET request failed: Item with {itemId} id not found.");
                throw new ArgumentException($"Given id {itemId} does not exist.");
            }

            var toDoItem = new ToDoItemView()
            {
                Id = response.ToDoItemId,
                Name = response.Name,
                Description = response.Description,
                IsCompleted = response.IsCompleted,
                Category = response.Category
            };
            return toDoItem;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occured: {e.Message}");
            return null;
        }
    }

    public async Task UpdateItemAsync(ToDoItemView item)
    {
        try
        {
            var itemRequest = new ToDoItemUpdateRequestDto(item.Name, item.Description, item.IsCompleted, item.Category);
            var response = await httpClient.PutAsJsonAsync($"api/ToDoItems/{item.Id}", itemRequest);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine($"PUT request successful: Updated ToDoItem with id {item.Id}.");
                return;
            }
            else
            {
                Console.WriteLine($"PUT request failed: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occured: {e.Message}");
        }

    }

    public async Task DeleteItemAsync(int itemId)
    {
        try
        {
            var response = await httpClient.DeleteAsync($"api/ToDoItems/{itemId}");
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                Console.WriteLine($"DELETE request successful: Deleted ToDoItem with id {itemId}.");
                return;
            }
            else
            {
                Console.WriteLine($"DELETE request failed with status code: {response.StatusCode}");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception occured: {e.Message}");
        }
    }
}
