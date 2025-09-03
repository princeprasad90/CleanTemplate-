using CleanTemplate.Application.Services;
using CleanTemplate.Domain.Events;
using Microsoft.AspNetCore.Mvc;

namespace CleanTemplate.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _service;

    public ProductsController(ProductService service) => _service = service;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateRequest request)
    {
        var id = await _service.CreateAsync(request.Name);
        return Ok(id);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRequest request)
    {
        await _service.UpdateAsync(id, request.Name);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/submit")]
    public async Task<IActionResult> Submit(Guid id)
    {
        await _service.SubmitAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(Guid id)
    {
        await _service.ApproveAsync(id);
        return NoContent();
    }

    [HttpPost("{id}/reject")]
    public async Task<IActionResult> Reject(Guid id, [FromBody] CommentRequest request)
    {
        await _service.RejectAsync(id, request.Comment);
        return NoContent();
    }

    [HttpPost("{id}/return")]
    public async Task<IActionResult> Return(Guid id, [FromBody] CommentRequest request)
    {
        await _service.ReturnAsync(id, request.Comment);
        return NoContent();
    }

    [HttpGet("{id}/events")]
    public async Task<IEnumerable<IEvent>> GetEvents(Guid id) =>
        await _service.GetEventsAsync(id);

    public record CreateRequest(string Name);
    public record UpdateRequest(string Name);
    public record CommentRequest(string Comment);
}
