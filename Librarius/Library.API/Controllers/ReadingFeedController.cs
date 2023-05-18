using Library.API.SignalRHubs;
using Library.API.SignalRHubs.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Library.API.Controllers;

[Route("[controller]")]
[ApiController]
public class ReadingFeedController : ControllerBase
{
    private readonly IHubContext<ReadingHub> _readingHubContext;

    public ReadingFeedController(IHubContext<ReadingHub> readingHubContext)
    {
        _readingHubContext = readingHubContext;
    }
    
    [HttpPost]
    public async Task<ActionResult> AddReading(ReadingFeedRequestModel reading)
    {
        readings.Add(reading);
        // Send the updated readings to connected clients via SignalR
        await _readingHubContext.Clients.All.SendAsync("ReceiveReadingUpdate", readings);
        return Ok();
    }
}

