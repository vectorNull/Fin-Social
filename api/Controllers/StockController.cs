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
    private readonly IStockRepo _stockRepo;
    private readonly AppDbContext _context;

    public StockController(AppDbContext context, IStockRepo stockRepo)
    {
      _context = context;
      _stockRepo = stockRepo;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var stocks = await _stockRepo.GetAllAsync();
      var stockDTOs = stocks.Select(s => s.ToStockDto());

      return Ok(stockDTOs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
      var stockModel = await _stockRepo.GetByIdAsync(id);
      
      if (stockModel == null)
      {
        return NotFound();
      }

      return Ok(stockModel.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
      var stockModel = stockDto.ToStockFromCreateDto();
      await _stockRepo.CreateAsync(stockModel);
      return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
    {
      var stockModel = await _stockRepo.UpdateAsync(id, stockDto);

      if (stockModel is null)
      {
        return NotFound();
      }

      return Ok(stockModel.ToStockDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
      var stockModel = await _stockRepo.DeleteAsync(id);

      if (stockModel == null)
      {
        return NotFound();
      }

      return NoContent();
    }
  }
}