using Blog.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Validators
{
    public class TagUpdateDtoValidator : AbstractValidator<TagUpdateDto>
    {
        public TagUpdateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tag adı boş olamaz.")
                .MinimumLength(2).WithMessage("Tag adı en az 2 karakter olmalı.")
                .MaximumLength(50).WithMessage("Tag adı 50 karakteri geçemez.");
        }
    }
}
