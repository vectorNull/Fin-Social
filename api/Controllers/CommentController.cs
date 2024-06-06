using api.DTOs.Comment;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/v1/comment")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepo _commentRepo;
        private readonly IStockRepo _stockRepo;

        public CommentController(ICommentRepo commentRepo, IStockRepo stockRepo)
        {
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCommentsAsync()
        {
            //! Uncomment if ever passing dto to method. Validation performed in class.
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

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
            //! Uncomment if ever passing dto to method. Validation performed in class.
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            var commentModel = await _commentRepo.GetByIdAsync(id);

            if (commentModel is null)
            {
                return NotFound();
            }

            return Ok(commentModel.ToCommentDto());
        }

        [HttpPost("{stockId:int}")]
        public async Task<IActionResult> CreateComment([FromRoute] int stockId, [FromBody] CreateCommentRequestDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _stockRepo.StockExists(stockId))
            {
                return BadRequest("Stock does not exist.");
            }

            var commentModel = commentDto.FromCreatedDtoToComment(stockId);

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
            //! Uncomment if ever passing dto to method. Validation performed in class.
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            var comment = await _commentRepo.DeleteAsync(id);

            if (comment is null)
            {
                return NotFound();
            }

            return NoContent();
        }

    }
}
