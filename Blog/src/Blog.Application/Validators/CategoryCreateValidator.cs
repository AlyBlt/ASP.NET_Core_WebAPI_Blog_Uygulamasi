using Blog.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Validators
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Kategori adı boş olamaz.")
                .MinimumLength(2).WithMessage("Kategori adı en az 2 karakter olmalı.")
                .MaximumLength(100).WithMessage("Kategori adı 100 karakteri geçemez.")
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Kategori adı sadece boşluk olamaz.");
        }
    }
}
