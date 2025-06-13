using Events_system.DTOs;
using FluentValidation;

namespace Events_system.Validation
{
    public class OrderCreateValidator : AbstractValidator<OrderCreateDTO>
    {
        public OrderCreateValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");
        }
    }
}
