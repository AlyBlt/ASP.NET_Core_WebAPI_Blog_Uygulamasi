using App.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using App.Api.DTOs;
using App.Api.Services.Interfaces;

namespace App.Api.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _service;
        private readonly ILogger<ArticlesController> _logger;

        public ArticlesController(IArticleService service, ILogger<ArticlesController> logger)
        {
            _service = service;
            _logger = logger;
        }

       
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ArticleReadDto>))]
        
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Tüm makaleleri getirme isteği alındı.");
            var articles = await _service.GetAllAsync();
            return Ok(articles);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Id'si {Id} olan makale getiriliyor.", id);
            var article = await _service.GetByIdAsync(id);
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            _logger.LogInformation("Yeni makale oluşturma isteği alındı: {@Dto}", dto);
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        
        public async Task<IActionResult> Update(int id, [FromBody] ArticleUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Id'si {Id} olan makale güncelleniyor.", id);
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Güncellenecek makale bulunamadı. Id: {Id}", id);
                return NotFound("Makale bulunamadı.");
            }

            await _service.UpdateAsync(id, dto);
            return NoContent();
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
       
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Id'si {Id} olan makale siliniyor.", id);
            var existing = await _service.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Silinecek makale bulunamadı. Id: {Id}", id);
                return NotFound("Makale bulunamadı.");
            }
            await _service.DeleteAsync(id);
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
