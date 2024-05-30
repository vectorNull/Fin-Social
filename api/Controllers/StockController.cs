using api.Data;
using api.DTOs.Stock;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
  [Route("api/v1/stock")]
  [ApiController]
  public class StockController : ControllerBase
  {
    private readonly IStockRepo _repo;
    private readonly AppDbContext _context;

    public StockController(AppDbContext context, IStockRepo repo)
    {
      _context = context;
      _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var stocks = await _repo.GetAllAsync();
      var stockDTOs = stocks.Select(s => s.ToStockDto());

      return Ok(stockDTOs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      var stock = await _repo.GetByIdAsync(id);
      
      if (stock == null)
      {
        return NotFound();
      }

      return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
      var stockModel = stockDto.ToStockFromCreateDto();
      await _repo.CreateAsync(stockModel);
      return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
    {
      var stock = await _repo.UpdateAsync(id, stockDto);

      if (stock is null)
      {
        return NotFound();
      }

      return Ok(stock.ToStockDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      var stock = await _repo.DeleteAsync(id);

      if (stock == null)
      {
        return NotFound();
      }

      return NoContent();
    }
  }
}