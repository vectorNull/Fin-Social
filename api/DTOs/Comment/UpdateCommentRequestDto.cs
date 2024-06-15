using System.ComponentModel.DataAnnotations;

namespace api.DTOs.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MaxLength(280, ErrorMessage = "Title cannot be more than 280 characters.")]
        [MinLength(5, ErrorMessage = "Title must be 5 characters.")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MaxLength(280, ErrorMessage = "Content cannot be more than 280 characters.")]
        [MinLength(5, ErrorMessage = "Content must be 5 characters.")]
        public string Content { get; set; } = string.Empty;
    }
}