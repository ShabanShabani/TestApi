using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Application.DTOs;

namespace ValidataApi.Application.Validators
{
    public class ItemDtoValidator : AbstractValidator<ItemDto>
    {
        public ItemDtoValidator()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required.")
                .MaximumLength(100).WithMessage("Product name cannot exceed 100 characters.");
            RuleFor(x => x.ProductPrice)
                .GreaterThan(0).WithMessage("Product price must be positive.");
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be at least 1.");
        }
    }
}
