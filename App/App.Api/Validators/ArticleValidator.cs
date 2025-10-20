using App.Api.DTOs;
using FluentValidation;

namespace App.Api.Validators
{
    public class ArticleValidator : AbstractValidator<ArticleCreateDto>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş olamaz.")
                .Length(5, 100).WithMessage("Başlık 5 ile 100 karakter arasında olmalıdır.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("İçerik boş olamaz.")
                .Length(10, 500).WithMessage("İçerik 10 ile 500 karakter arasında olmalıdır.");
        }
    }
}
