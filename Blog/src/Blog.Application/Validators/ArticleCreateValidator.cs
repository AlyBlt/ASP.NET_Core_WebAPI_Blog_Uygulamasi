using Blog.Application.DTOs;
using FluentValidation;

namespace Blog.Application.Validators
{
    public class ArticleCreateValidator : AbstractValidator<ArticleCreateDto>
    {
        public ArticleCreateValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .MinimumLength(5).WithMessage("Başlık en az 5 karakter olmalı.")
                .MaximumLength(250).WithMessage("Başlık 250 karakteri geçemez.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Başlık sadece boşluk olamaz.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz.")
                .MinimumLength(10).WithMessage("İçerik en az 10 karakter olmalı.")
                .MaximumLength(5000).WithMessage("İçerik 5000 karakteri geçemez.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("İçerik sadece boşluk olamaz.");

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Geçerli bir kategori seçilmelidir.");

            RuleForEach(x => x.TagIds)
                .GreaterThan(0).WithMessage("Geçerli tag seçilmelidir.");

            // Enum validation
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Geçersiz makale durumu.");
        }
    }
}
