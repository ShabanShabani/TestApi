using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidataApi.Application.Commands;

namespace ValidataApi.Application.Validators
{
    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .GreaterThan(0).WithMessage("Customer ID must be valid.");
            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage("Order date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Order date cannot be in the future.");
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item is required.")
                .ForEach(item => item.SetValidator(new ItemDtoValidator()));
        }
    }
}
