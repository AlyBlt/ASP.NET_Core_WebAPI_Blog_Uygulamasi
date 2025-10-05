using App.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Article>))]
        public IActionResult GetList()
        {
            return Ok(articles);
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Article))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string)) ]
        public IActionResult GetArticle(int id)
        {
            var article = articles.Find(x => x.Id == id);
            if (article == null)
                return NotFound();
            return Ok(article);

        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Article))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [Consumes(typeof(Article), "application/json")]
        public IActionResult CreateArticle(Article article)
        {
            if (string.IsNullOrWhiteSpace(article.Title))
                return BadRequest("Makale başlığı boş olamaz.");
             var item = new Article
            {
                Id = _nextId,
                Title = article.Title,
                Content = article.Content
            };
            _nextId++;
            articles.Add(item);
            return CreatedAtAction(nameof(GetArticle), new { id = item.Id }, new
            {
                message = "Yeni makale oluşturuldu.",
                data = item
            });
        }


        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Article))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Consumes(typeof(Article), "application/json")]
        public IActionResult UpdateArticle(int id, Article article)
        {
            if (string.IsNullOrWhiteSpace(article.Title))
                return BadRequest("Makale başlığı boş olamaz.");

            var index = articles.FindIndex(x => x.Id == id);
            if (index == -1)
                return NotFound("Makale bulunamadı.");
  
            articles[index].Title = article.Title;
            articles[index].Content = article.Content;

            return Ok(new
            {
                message = "Makale güncellendi.",
                data = articles[index]
            });

        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteArticle(int id)
        {
            var index = articles.FindIndex(x => x.Id == id);

            if (index == -1)
                return NoContent();

            articles.RemoveAt(index);

            return Ok(new
            {
                message = "Makale silindi.",
                data = articles[index]

            });  


        }




    }
}
