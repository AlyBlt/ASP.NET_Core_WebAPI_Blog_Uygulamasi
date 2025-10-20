using App.Api.DTOs;
using App.Api.Models;
using App.Api.Services.Implementations;
using App.Api.Services.Interfaces;
using App.Api.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;


namespace App.Api.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly IValidator<ArticleCreateDto> _articleValidator;
        private readonly IValidator<ArticleUpdateDto> _articleUpdateValidator;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(IArticleService articleService,
                          IValidator<ArticleCreateDto> articleValidator,
                          IValidator<ArticleUpdateDto> articleUpdateValidator,
                          ILogger<ArticlesController> logger)
        {
            _articleService = articleService;
            _articleValidator = articleValidator;
            _articleUpdateValidator = articleUpdateValidator;
            _logger = logger;
        }

       
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ArticleReadDto>))]
        
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm makaleleri getirme isteği alındı.");
            var articles = await _articleService.GetAllAsync();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Id'si {Id} olan makale getiriliyor.", id);
            var article = await _articleService.GetByIdAsync(id);
            if (article == null)
            {
                _logger.LogWarning("Id'si {Id} olan makale bulunamadı.", id);
                return NotFound("Makale bulunamadı.");
            }
            return Ok(article);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Consumes("application/json")]
        
        public async Task<IActionResult> Create([FromBody] ArticleCreateDto dto)
        {
            // Validate the DTO
            var validationResult = await _articleValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var createdArticle = await _articleService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = createdArticle.Id }, createdArticle);

           
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        
        public async Task<IActionResult> Update(int id, [FromBody] ArticleUpdateDto dto)
        {
            // FluentValidation kontrolü
            var validationResult = await _articleUpdateValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            _logger.LogInformation("Id'si {Id} olan makale güncelleniyor.", id);
            var existing = await _articleService.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Güncellenecek makale bulunamadı. Id: {Id}", id);
                return NotFound("Makale bulunamadı.");
            }

            await _articleService.UpdateAsync(id, dto);
            return NoContent();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
       
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Id'si {Id} olan makale siliniyor.", id);
            var existing = await _articleService.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Silme işlemi başarısız. Id {Id} bulunamadı.", id);
                return NotFound("Makale bulunamadı.");
            }
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
