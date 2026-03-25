using System.Diagnostics;
using CarMarketplace.Api.Common;
using CarMarketplace.Api.Dtos;
using CarMarketplace.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarMarketplace.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CarsController(ICarService carService, ICurrentUserService currentUserService) : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PagedResponse<CarListingResponseDto>>>> Search([FromQuery] CarSearchRequestDto request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var cars = await carService.SearchPublicAsync(request, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[CarsController.Search] Execution Time: {sw.ElapsedMilliseconds}ms");
        return Ok(ApiResponse<PagedResponse<CarListingResponseDto>>.SuccessResponse(cars));
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<CarListingResponseDto>>> GetById(int id, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var car = await carService.GetPublicByIdAsync(id, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[CarsController.GetById] Execution Time: {sw.ElapsedMilliseconds}ms");
        return Ok(ApiResponse<CarListingResponseDto>.SuccessResponse(car));
    }

    [HttpPost]
    [Authorize(Roles = AppConstants.Roles.Seller)]
    public async Task<ActionResult<ApiResponse<CarListingResponseDto>>> Create(CreateCarRequestDto request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var car = await carService.CreateAsync(currentUserService.UserId, request, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[CarsController.Create] Execution Time: {sw.ElapsedMilliseconds}ms");
        return CreatedAtAction(nameof(GetById), new { id = car.Id }, ApiResponse<CarListingResponseDto>.SuccessResponse(car, "Car listing created successfully."));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = $"{AppConstants.Roles.Seller},{AppConstants.Roles.Admin}")]
    public async Task<ActionResult<ApiResponse<CarListingResponseDto>>> Update(int id, UpdateCarRequestDto request, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var car = await carService.UpdateAsync(id, currentUserService.UserId, currentUserService.Role, request, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[CarsController.Update] Execution Time: {sw.ElapsedMilliseconds}ms");
        return Ok(ApiResponse<CarListingResponseDto>.SuccessResponse(car, "Car listing updated successfully."));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = $"{AppConstants.Roles.Seller},{AppConstants.Roles.Admin}")]
    public async Task<ActionResult<ApiResponse<object>>> Delete(int id, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        await carService.DeleteAsync(id, currentUserService.UserId, currentUserService.Role, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[CarsController.Delete] Execution Time: {sw.ElapsedMilliseconds}ms");
        return Ok(ApiResponse<object>.SuccessResponse(null, "Car listing deleted successfully."));
    }

    [HttpGet("mine")]
    [Authorize(Roles = AppConstants.Roles.Seller)]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<CarListingResponseDto>>>> Mine(CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();
        var cars = await carService.GetMineAsync(currentUserService.UserId, cancellationToken);
        sw.Stop();
        Console.WriteLine($"[CarsController.Mine] Execution Time: {sw.ElapsedMilliseconds}ms");
        return Ok(ApiResponse<IReadOnlyCollection<CarListingResponseDto>>.SuccessResponse(cars));
    }
}
