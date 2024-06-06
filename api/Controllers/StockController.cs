using api.Data;
using api.DTOs.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using MethodTimer;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/v1/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRepo _stockRepo;

        //* Used for unit testing
        public StockController(IStockRepo stockRepo)
        {
            _stockRepo = stockRepo;
        }

        [Time]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] QueryObject query)
        {

            //! Uncomment if ever passing dto to method. Validation performed in class.
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDTOs = stocks.Select(s => s.ToStockDto());

            return Ok(stockDTOs);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            //! Uncomment if ever passing dto to method. Validation performed in class.
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stockModel = await _stockRepo.UpdateAsync(id, stockDto);

            if (stockModel is null)
            {
                return NotFound();
            }

            return Ok(stockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //! Uncomment if ever passing dto to method. Validation performed in class.
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            var stockModel = await _stockRepo.DeleteAsync(id);

            if (stockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
