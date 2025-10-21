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
using AutoMapper;


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
        private readonly IMapper _mapper;

        public ArticlesController(IArticleService articleService,
                          IValidator<ArticleCreateDto> articleValidator,
                          IValidator<ArticleUpdateDto> articleUpdateValidator,
                          ILogger<ArticlesController> logger,
                          IMapper mapper)
        {
            _articleService = articleService;
            _articleValidator = articleValidator;
            _articleUpdateValidator = articleUpdateValidator;
            _logger = logger;
            _mapper = mapper;
        }

       
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ArticleReadDto>))]
        
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm makaleleri getirme isteği alındı.");
            var articles = await _articleService.GetAllAsync();

            // AutoMapper kullanarak Article modelinden ArticleReadDto'ya dönüştür
            var articleDtos = _mapper.Map<IEnumerable<ArticleReadDto>>(articles);
            return Ok(articleDtos);
           
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
            // AutoMapper kullanarak Article modelini ArticleReadDto'ya dönüştür
            var articleDto = _mapper.Map<ArticleReadDto>(article);
            return Ok(articleDto);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Consumes("application/json")]
        
        public async Task<IActionResult> Create([FromBody] ArticleCreateDto dto)
        {
            // Validate the DTO/Fluent Validation
            var validationResult = await _articleValidator.ValidateAsync(dto);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            // Yeni makaleyi oluştur
            var createdArticle = await _articleService.CreateAsync(dto);
               
            // Başarıyla oluşturulduğunda dönecek yer
            return CreatedAtAction(nameof(GetById), new { id = createdArticle.Id }, createdArticle);
            
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        
        public async Task<IActionResult> Update(int id, [FromBody] ArticleUpdateDto dto)
        {
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
            var updatedArticle = await _articleService.GetByIdAsync(id);
            return Ok(updatedArticle); // 200 OK ve güncellenmiş veriyi döndür

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
