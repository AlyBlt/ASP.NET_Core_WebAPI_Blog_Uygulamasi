using App.Api.Models;
using App.Api.Repositories.Interfaces;

namespace App.Api.Repositories.Implementations
{
    public class ArticleRepository : IArticleRepository
    {
        private static List<Article> _articles = new List<Article>
        {
            new Article { Id = 1, Title = "ASP.NET Core ile Web API Geliştirme", Content = "Bu makalede ASP.NET Core ile Web API geliştirmenin temellerini öğreneceksiniz.", CreatedAt = DateTime.UtcNow },
            new Article { Id = 2, Title = "Postman Kullanımı", Content = "Postman ile API testlerini nasıl yapabileceğinizi öğreneceksiniz.", CreatedAt = DateTime.UtcNow }
        };

        private static int _nextId = 3;

        public async Task<IEnumerable<Article>> GetAllAsync()
        {
            return await Task.FromResult(_articles);
        }

        public async Task<Article> GetByIdAsync(int id)
        {
            var article = _articles.FirstOrDefault(a => a.Id == id);
            return await Task.FromResult(article);
        }

        public async Task<Article> CreateAsync(Article article)
        {
            article.Id = _nextId++;
            article.CreatedAt = DateTime.UtcNow;
            _articles.Add(article);
            return await Task.FromResult(article);
        }

        public async Task UpdateAsync(Article article)
        {
            var index = _articles.FindIndex(a => a.Id == article.Id);
            if (index != -1)
            {
                _articles[index] = article;
            }

            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Article article)
        {
            _articles.Remove(article);
            await Task.CompletedTask;
        }
    }
}