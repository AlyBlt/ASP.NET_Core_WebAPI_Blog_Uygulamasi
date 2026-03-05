using Blog.Application.DTOs.Comment;
using FluentValidation;


namespace Blog.Application.Validators.Comment
{
    public class CommentCreateDtoValidator : AbstractValidator<CommentCreateDto>
    {
        public CommentCreateDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Yorum içeriği boş olamaz.")
                .MinimumLength(1).WithMessage("Yorum en az 1 karakter olmalı.")
                .MaximumLength(1000).WithMessage("Yorum 1000 karakteri geçemez.");

            RuleFor(x => x.ArticleId)
                .GreaterThan(0).WithMessage("Geçerli bir makale seçilmelidir.");
        }
    }
}
