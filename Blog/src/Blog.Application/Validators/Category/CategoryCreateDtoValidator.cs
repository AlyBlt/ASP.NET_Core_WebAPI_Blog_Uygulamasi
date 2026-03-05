using Blog.Application.DTOs.Category;
using FluentValidation;


namespace Blog.Application.Validators.Category
{
    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı boş olamaz.")
                .MinimumLength(2).WithMessage("Kategori adı en az 2 karakter olmalı.")
                .MaximumLength(100).WithMessage("Kategori adı 100 karakteri geçemez.");

            // Slug alanı için kural 
            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug alanı boş olamaz.")
                .Matches(@"^[a-z0-9-]+$").WithMessage("Slug sadece küçük harf, rakam ve tire içerebilir.");
        }
    }
}
