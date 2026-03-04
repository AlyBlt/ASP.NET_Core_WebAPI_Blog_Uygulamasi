using Blog.Application.DTOs;
using Blog.Application.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IValidator<ArticleCreateDto> _articleValidator;
        private readonly IValidator<ArticleUpdateDto> _articleUpdateValidator;
        private readonly ILogger<ArticlesController> _logger;
        private readonly IUserService _userService;
        public ArticlesController(IArticleService articleService,
                          IValidator<ArticleCreateDto> articleValidator,
                          IValidator<ArticleUpdateDto> articleUpdateValidator,
                          ILogger<ArticlesController> logger,
                          IUserService userService)
        {
            _articleService = articleService;
            _articleValidator = articleValidator;
            _articleUpdateValidator = articleUpdateValidator;
            _logger = logger;
            _userService = userService;
        }

        [Authorize]  // Bu action sadece authenticated kullanıcılar için geçerli
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ArticleReadDto>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Yetkisiz erişim için
        public async Task<IActionResult> GetAll()
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            if (user == null)
                return Unauthorized("Geçersiz kullanıcı.");

            _logger.LogInformation("Tüm makaleleri getirme isteği alındı.");
            var articles = await _articleService.GetAllAsync();

            return Ok(articles); // DTO zaten service'ten geliyor

        }

        [Authorize]  // Bu action sadece authenticated kullanıcılar için geçerli
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] // Yetkisiz erişim için
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            if (user == null)
                return Unauthorized("Geçersiz kullanıcı.");

            _logger.LogInformation("Id'si {Id} olan makale getiriliyor.", id);
            var article = await _articleService.GetByIdAsync(id);
            if (article == null)
            {
                _logger.LogWarning("Id'si {Id} olan makale bulunamadı.", id);
                return NotFound("Makale bulunamadı.");
            }
           
            return Ok(article);
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/json")]
        
        public async Task<IActionResult> Create([FromBody] ArticleCreateDto dto)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            if (user == null)
                return Unauthorized("Geçersiz kullanıcı.");

            // Validate the DTO/Fluent Validation
            var validationResult = await _articleValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            // Yeni makaleyi oluştur
            var createdArticle = await _articleService.CreateAsync(dto, user.Id);
               
            // Başarıyla oluşturulduğunda dönecek yer
            return CreatedAtAction(nameof(GetById), new { id = createdArticle.Id }, createdArticle);
            
        }

        [Authorize(Roles = "Admin,Author")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Consumes("application/json")]
        
        public async Task<IActionResult> Update(int id, [FromBody] ArticleUpdateDto dto)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            if (user == null)
                return Unauthorized("Geçersiz kullanıcı.");

            // DTO'yu doğrula
            var validationResult = await _articleUpdateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            // Veritabanındaki mevcut makaleyi al
            var existing = await _articleService.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Güncellenecek makale bulunamadı. Id: {Id}", id);
                return NotFound("Makale bulunamadı.");
            }

            // Loglama işlemi isteğe bağlı
            _logger.LogInformation("Id'si {Id} olan makale güncelleniyor.", id);

            
            // Makaleyi güncelle
            await _articleService.UpdateAsync(id, dto);

            // Güncellenmiş makale verisini döndür
            var updatedArticle = await _articleService.UpdateAsync(id, dto);
            if (updatedArticle == null) return NotFound("Makale bulunamadı.");
            return Ok(updatedArticle);

        }

        [Authorize(Roles = "Admin,Author")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
       
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.GetCurrentUserAsync(HttpContext);
            if (user == null)
                return Unauthorized("Geçersiz kullanıcı.");

            _logger.LogInformation("Id'si {Id} olan makale siliniyor.", id);
            var existing = await _articleService.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Silme işlemi başarısız. Id {Id} bulunamadı.", id);
                return NotFound("Makale bulunamadı.");
            }

            // Author sadece kendi makalesini silebilir
            if (user.Role == "Author" && existing.UserId != user.Id)
                return Forbid();

            await _articleService.DeleteAsync(id);
            return NoContent();
        }

        ////Exception deneme
        //[HttpGet("throw")]
        //public IActionResult ThrowError()
        //{
        //    throw new Exception("Bu test amaçlı bir exception'dır.");
        //}


    }
}
