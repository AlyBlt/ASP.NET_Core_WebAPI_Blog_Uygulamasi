using Blog.Application.DTOs.Auth;
using FluentValidation;

namespace Blog.Application.Validators.Auth
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalı.")
                .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakteri geçemez.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Kullanıcı adı sadece boşluk olamaz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı.")
                .MaximumLength(100).WithMessage("Şifre 100 karakteri geçemez.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Şifre sadece boşluk olamaz.");
        }
    }
}