using System.Diagnostics;
using CarMarketplace.Api.Common;
using CarMarketplace.Api.Dtos;
using CarMarketplace.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarMarketplace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController(IOrderService orderService, ICurrentUserService currentUserService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = AppConstants.Roles.Buyer)]
    public async Task<ActionResult<ApiResponse<OrderResponseDto>>> Create(CreateOrderRequestDto request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var order = await orderService.CreateAsync(currentUserService.UserId, request, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[OrdersController.Create] Execution Time: {sw.ElapsedMilliseconds}ms");
        return CreatedAtAction(nameof(Mine), ApiResponse<OrderResponseDto>.SuccessResponse(order, "Booking request created successfully."));
    }

    [HttpGet("mine")]
    [Authorize(Roles = AppConstants.Roles.Buyer)]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<OrderResponseDto>>>> Mine(CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var orders = await orderService.GetMineAsync(currentUserService.UserId, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[OrdersController.Mine] Execution Time: {sw.ElapsedMilliseconds}ms");
        return Ok(ApiResponse<IReadOnlyCollection<OrderResponseDto>>.SuccessResponse(orders));
    }

    [HttpGet("seller")]
    [Authorize(Roles = AppConstants.Roles.Seller)]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<OrderResponseDto>>>> Seller(CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var orders = await orderService.GetSellerOrdersAsync(currentUserService.UserId, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[OrdersController.Seller] Execution Time: {sw.ElapsedMilliseconds}ms");
        return Ok(ApiResponse<IReadOnlyCollection<OrderResponseDto>>.SuccessResponse(orders));
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = $"{AppConstants.Roles.Seller},{AppConstants.Roles.Admin}")]
    public async Task<ActionResult<ApiResponse<OrderResponseDto>>> UpdateStatus(int id, UpdateOrderStatusRequestDto request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var order = await orderService.UpdateStatusAsync(id, currentUserService.UserId, currentUserService.Role, request, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[OrdersController.UpdateStatus] Execution Time: {sw.ElapsedMilliseconds}ms");
        return Ok(ApiResponse<OrderResponseDto>.SuccessResponse(order, "Order status updated successfully."));
    }
}
