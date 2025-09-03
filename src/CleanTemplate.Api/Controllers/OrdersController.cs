using CleanTemplate.Application.Services;
using CleanTemplate.Domain.Events;
using Microsoft.AspNetCore.Mvc;

namespace CleanTemplate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;

    public OrdersController(OrderService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create()
    {
        var id = await _service.CreateAsync();
        return Ok(id);
    }

    [HttpPost("{id}/items")]
    public async Task<IActionResult> AddItem(Guid id, [FromBody] AddItemRequest request)
    {
        await _service.AddItemAsync(id, request.Product, request.Quantity);
        return NoContent();
    }

    [HttpGet("{id}/events")]
    public async Task<IEnumerable<IEvent>> GetEvents(Guid id) =>
        await _service.GetEventsAsync(id);

    public record AddItemRequest(string Product, int Quantity);
}
