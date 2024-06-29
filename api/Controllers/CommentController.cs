using api.DTOs.Comment;
using api.Extensions;
using api.Interfaces;
using api.Mappers;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v1/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepo _commentRepo;
        private readonly IStockRepo _stockRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IFinancialModelingPrepService _financialModelingPrepService;

        public CommentController(ICommentRepo commentRepo, IStockRepo stockRepo, UserManager<AppUser> userManager, IFinancialModelingPrepService financialModelingPrepService)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
            _userManager = userManager;
            _financialModelingPrepService = financialModelingPrepService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCommentsAsync()
        {
            var comments = await _commentRepo.GetAllAsync();

            if (comments is null)
            {
                return NotFound();
            }

            var commentDtos = comments.Select(c => c.ToCommentDto());

            return Ok(commentDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCommentbyId([FromRoute] int id)
        {
            var commentModel = await _commentRepo.GetByIdAsync(id);

            if (commentModel is null)
            {
                return NotFound();
            }

            return Ok(commentModel.ToCommentDto());
        }

        [HttpPost]
        [Route("{symbol:alpha}")]
        public async Task<IActionResult> CreateComment([FromRoute] string symbol, [FromBody] CreateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stock = await _stockRepo.GetBySymbolAsync(symbol);

            if (stock is null)
            {
                stock = await _financialModelingPrepService.FindStockBySymbolAsync(symbol);
                
                if (stock is null)
                {
                    return BadRequest("Stock not found");
                }
                else
                {
                    await _stockRepo.CreateAsync(stock);
                }
            }

            var username = User.GetUsername();

            if (username == null)
            {
                return Unauthorized();
            }

            var appUser = await _userManager.FindByNameAsync(username);

            if (appUser == null)
            {
                return NotFound();
            }

            var commentModel = commentDto.FromCreatedDtoToComment(stock.Id, appUser.Id);

            await _commentRepo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetCommentbyId), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var comment = await _commentRepo.UpdateAsync(id, commentDto);

            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var comment = await _commentRepo.DeleteAsync(id);

            if (comment is null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}