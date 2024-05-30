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
        private readonly ICommentRepo _repo;

        public CommentController(ICommentRepo repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCommentsAsync()
        {
            var comments = await _repo.GetAllAsync();

            if (comments is null)
            {
                return NotFound();
            }

            var commentDtos = comments.Select(c => c.ToCommentDto());

            return Ok(commentDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentbyId([FromRoute] int id)
        {
            var comment = await _repo.GetByIdAsync(id);

            if (comment is null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());            
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequestDto commentDto)
        {
            var commentModel = commentDto.FromCreatedDtoToComment();

            await _repo.CreateAsync(commentModel);

            return CreatedAtAction(nameof(GetCommentbyId), new { id = commentModel.Id }, commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequestDto commentDto)
        {
            var comment = await _repo.UpdateAsync(id, commentDto);

            if (comment is null)
            {
                return NotFound();
            }

            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteComment([FromRoute] int id)
        {
            var comment = await _repo.DeleteAsync(id);

            if (comment is null)
            {
                return NotFound();
            }

            return NoContent();
        }
        
    }
}