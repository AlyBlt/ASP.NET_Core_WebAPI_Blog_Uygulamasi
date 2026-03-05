using Blog.Application.DTOs;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Blog.Application.Validators
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Kullanıcı adı boş olamaz.")
                .MinimumLength(3).WithMessage("Kullanıcı adı en az 3 karakter olmalı.")
                .MaximumLength(50).WithMessage("Kullanıcı adı 50 karakteri geçemez.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Kullanıcı adı sadece boşluk olamaz.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş olamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalı.")
                .MaximumLength(100).WithMessage("Şifre 100 karakteri geçemez.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Şifre sadece boşluk olamaz.")
                .Matches(@"[A-Z]+").WithMessage("Şifre en az 1 büyük harf içermelidir.")
                .Matches(@"[a-z]+").WithMessage("Şifre en az 1 küçük harf içermelidir.")
                .Matches(@"[0-9]+").WithMessage("Şifre en az 1 rakam içermelidir.")
                .Matches(@"[\!\@\#\$\%\^\&\*\(\)\-\+\=]+").WithMessage("Şifre en az 1 özel karakter içermelidir (!@#$%^&*()-+=).");
        }
    }
}