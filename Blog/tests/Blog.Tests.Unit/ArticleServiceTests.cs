using AutoMapper;
using Blog.Application.DTOs.Article; // DTO namespace'ini kontrol et
using Blog.Application.Interfaces.Repositories;
using Blog.Application.Services;
using Blog.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Blog.Tests.Unit
{
    public class ArticleServiceTests
    {
        // Baðýmlýlýklarý field olarak tanýmlýyoruz ki her testte kullanabilelim
        private readonly Mock<IArticleRepository> _mockRepo;
        private readonly Mock<ILogger<ArticleService>> _mockLogger;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ArticleService _articleService;

        public ArticleServiceTests()
        {
            // 1. Mock nesnelerini oluþturuyoruz
            _mockRepo = new Mock<IArticleRepository>();
            _mockLogger = new Mock<ILogger<ArticleService>>();
            _mockMapper = new Mock<IMapper>();
           

            // 2. Servisi tüm mock baðýmlýlýklarýyla ayaða kaldýrýyoruz
            _articleService = new ArticleService(
                _mockRepo.Object,
                _mockLogger.Object,
                _mockMapper.Object
               
            );
        }

        [Fact]
        public async Task GetArticleById_ShouldReturnDto_WhenArticleExists()
        {
            var articleId = 1;
            var fakeEntity = new ArticleEntity { Id = articleId, Title = "Test" };
            var fakeDto = new ArticleReadDto { Id = articleId, Title = "Test" };

            _mockRepo.Setup(repo => repo.GetByIdAsync(articleId)).ReturnsAsync(fakeEntity);
            _mockMapper.Setup(m => m.Map<ArticleReadDto>(fakeEntity)).Returns(fakeDto);

            var result = await _articleService.GetByIdAsync(articleId);

            Assert.NotNull(result);
            Assert.Equal("Test", result.Title);
        }

        [Fact]
        public async Task GetById_WhenArticleDoesNotExist_ShouldReturnNull()
        {
            _mockRepo.Setup(x => x.GetByIdAsync(It.IsAny<int>()))
                     .ReturnsAsync((ArticleEntity)null);

            var result = await _articleService.GetByIdAsync(999);

            result.Should().BeNull();
        }
    }
}