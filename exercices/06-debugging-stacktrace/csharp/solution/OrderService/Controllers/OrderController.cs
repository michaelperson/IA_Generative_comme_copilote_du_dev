using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Services;

namespace OrderService.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly Services.OrderService _orderService;

    public OrderController(Services.OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPatch("{id}/status")]
    public async Task<IActionResult> PatchStatus(Guid id, [FromBody] OrderStatus status)
    {
        await _orderService.UpdateOrderStatusAsync(id, status);
        return NoContent();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSnapshot(Guid id)
    {
        var snapshot = await _orderService.GetOrderSnapshotAsync(id);
        return snapshot is null ? NotFound() : Ok(snapshot);
    }
}
