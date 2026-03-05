using Blog.Application.DTOs;
using FluentValidation;

namespace Blog.Application.Validators
{
    public class CommentUpdateValidator : AbstractValidator<CommentUpdateDto>
    {
        public CommentUpdateValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Yorum içeriği boş olamaz.")
                .MinimumLength(1).WithMessage("Yorum en az 1 karakter olmalı.")
                .MaximumLength(1000).WithMessage("Yorum 1000 karakteri geçemez.");
        }
    }
}
