using App.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using App.Api.DTOs;

namespace App.Api.Controllers
{
    [Route("api/articles")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private static List<Article> articles = new List<Article>
        {
            new Article { Id = 1, Title = "ASP.NET Core ile Web API Geliştirme", Content = "Bu makalede ASP.NET Core ile Web API geliştirmenin temellerini öğreneceksiniz." },
            new Article { Id = 2, Title = "Postman Kullanımı", Content = "Postman ile API testlerini nasıl yapabileceğinizi öğreneceksiniz." }
        };

        private static int _nextId = 3;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<ArticleReadDto>))]
        public IActionResult GetList()
        {
            //return Ok(articles);
            var result = articles.Select(x => new ArticleReadDto
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                CreatedAt = x.CreatedAt
            }).ToList();

            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
        public IActionResult GetArticle(int id)
        {
            var article = articles.Find(x => x.Id == id);
            if (article == null)
                return NotFound("Makale bulunamadı.");
            //if (article == null)
            //    return NotFound("Makale bulunamadı.");
            //return Ok(article);
            var result = new ArticleReadDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                CreatedAt = article.CreatedAt
            };
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Consumes("application/json")]
        public IActionResult CreateArticle(ArticleCreateDto article)
        {
            //if (string.IsNullOrWhiteSpace(article.Title))
            //    return BadRequest("Makale başlığı boş olamaz.");

            if (!ModelState.IsValid)            
            {
                return BadRequest(ModelState);
            }

            var item = new Article
            {
                Id = _nextId,
                Title = article.Title,
                Content = article.Content,
                CreatedAt = DateTime.UtcNow
            };
            _nextId++;
            articles.Add(item);

            var result = new ArticleReadDto
            {
                Id = item.Id,
                Title = item.Title,
                Content = item.Content,
                CreatedAt = item.CreatedAt
            };

            return CreatedAtAction(nameof(GetArticle), new { id = item.Id }, result);
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ArticleReadDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes("application/json")]
        public IActionResult UpdateArticle(int id, [FromBody] ArticleUpdateDto article)
        {
            //if (string.IsNullOrWhiteSpace(article.Title))
            //    return BadRequest("Makale başlığı boş olamaz.");

            if (!ModelState.IsValid)            
            {
                return BadRequest(ModelState);
            }

            var index = articles.FindIndex(x => x.Id == id);
            if (index == -1)
                return NotFound("Makale bulunamadı.");
  
            articles[index].Title = article.Title;
            articles[index].Content = article.Content;

            var updatedArticle = articles[index];
            var result = new ArticleReadDto
            {
                Id = updatedArticle.Id,
                Title = updatedArticle.Title,
                Content = updatedArticle.Content,
                CreatedAt = updatedArticle.CreatedAt
            };
            return Ok(new
            {
                message = "Makale güncellendi.",
                data = result
            });

        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteArticle(int id)
        {
            var article = articles.FirstOrDefault(x => x.Id == id);

            if (article == null)
                return NotFound("Makale bulunamadı.");

            articles.Remove(article);

            return NoContent();

        }

    }
}
