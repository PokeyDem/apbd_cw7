using Apbd_cw7.Models.DTOs;
using Apbd_cw7.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Apbd_cw7.Controller;

[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    private readonly IWarehousesRepository _warehousesRepository;
    public WarehousesController(IWarehousesRepository warehousesRepository)
    {
        _warehousesRepository = warehousesRepository;
    }
    
    [HttpPost]
    [Route("api/warehouses")]
    public async Task<IActionResult> RefillProducts(WarehouseProductDTO warehouseProductDto)
    {
        if (!await _warehousesRepository.DoesProductExists(warehouseProductDto.IdProduct))
            return NotFound($"Product with id: {warehouseProductDto.IdProduct} not found");

        if (!await _warehousesRepository.DoesWarehouseExists(warehouseProductDto.IdWarehouse))
            return NotFound($"Warehouse with id: {warehouseProductDto.IdWarehouse} not found");

        if (!_warehousesRepository.CheckAmount(warehouseProductDto.Amount))
            return BadRequest("Product amount cant be 0 or less");

        if (!await _warehousesRepository.DoesOrderExists(warehouseProductDto.IdProduct, warehouseProductDto.Amount,
                warehouseProductDto.CreatedAt))
            return NotFound("Order for those parameters not found");
        
        await _warehousesRepository.RefillProducts(warehouseProductDto);

        return Ok();
    }
}