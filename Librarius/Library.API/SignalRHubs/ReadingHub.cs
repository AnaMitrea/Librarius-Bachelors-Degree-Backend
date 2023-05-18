using Library.API.SignalRHubs.Model;
using Library.Application.Services;

namespace Library.API.SignalRHubs;

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ReadingHub : Hub
{
    private readonly IBookService _bookService;

    public ReadingHub(IBookService bookService)
    {
        _bookService = bookService;
    }
    
    public async Task SubscribeToReadingUpdates()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "ReadingUpdates");
    }

    public async Task UnsubscribeFromReadingUpdates()
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, "ReadingUpdates");
    }

    public async Task SendReadingUpdate(ReadingFeedRequestModel reading)
    {
        await Clients.Group("ReadingUpdates").SendAsync("ReceiveReadingUpdate", reading);
    }
    
    public async Task AddReading(ReadingFeedRequestModel reading)
    {
        // Logic to add the reading to your microservice's data store or perform any necessary operations
        // For demonstration purposes, let's assume you have a service called ReadingService that handles reading-related operations.
        // You can inject the ReadingService into the hub class using the DI framework of your choice (e.g., ASP.NET Core DI).

        // Assuming ReadingService has a method called AddReadingAsync to add the reading.
        await _bookService.AddReadingAsync(reading.Id);

        // Broadcast the reading update to connected clients
        await SendReadingUpdate(reading);
    }

    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();

        // Add the client to the "ReadingUpdates" group by default
        await SubscribeToReadingUpdates();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);

        // Remove the client from the "ReadingUpdates" group
        await UnsubscribeFromReadingUpdates();
    }
}
