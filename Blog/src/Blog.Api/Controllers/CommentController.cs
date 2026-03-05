using Blog.Application.DTOs.Comment;
using Blog.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService _commentService;
        private readonly IValidator<CommentCreateDto> _createValidator;
        private readonly IValidator<CommentUpdateDto> _updateValidator;
        private readonly IUserService _userService;

        public CommentController(
            ICommentService commentService,
            IValidator<CommentCreateDto> createValidator,
            IValidator<CommentUpdateDto> updateValidator,
            IUserService userService)
        {
            _commentService = commentService;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _userService = userService;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentService.GetAllAsync();
            return Ok(comments);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null) return NotFound("Yorum bulunamadı.");
            return Ok(comment);
        }

        [Authorize]
        [HttpGet("article/{articleId}")]
        public async Task<IActionResult> GetByArticleId(int articleId)
        {
            var comments = await _commentService.GetByArticleIdAsync(articleId);
            return Ok(comments);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentCreateDto dto)
        {
            var user = await _userService.GetCurrentUserAsync(User);
            if (user == null) return Unauthorized();

            var validation = await _createValidator.ValidateAsync(dto);
            if (!validation.IsValid) return BadRequest(validation.Errors);

            var created = await _commentService.CreateAsync(dto, user.Id);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CommentUpdateDto dto)
        {
            var user = await _userService.GetCurrentUserAsync(User);
            if (user == null) return Unauthorized();

            var validation = await _updateValidator.ValidateAsync(dto);
            if (!validation.IsValid) return BadRequest(validation.Errors);

            var updated = await _commentService.UpdateAsync(id, dto);
            if (updated == null) return NotFound("Yorum bulunamadı.");

            // Yalnızca kendi yorumunu güncelleyebilir
            if (updated.UserId != user.Id) return Forbid();

            return Ok(updated);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetCurrentUserAsync(User);
            if (user == null) return Unauthorized();

            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null) return NotFound("Yorum bulunamadı.");

            // Sadece kendi yorumunu veya admin silebilir
            if (comment.UserId != user.Id && user.Role != Blog.Domain.Enums.UserRole.Admin)
                return Forbid();

            await _commentService.DeleteAsync(id);
            return NoContent();
        }
    }
}