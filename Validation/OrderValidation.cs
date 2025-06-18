using Events_system.DTOs;
using FluentValidation;

namespace Events_system.Validation
{
    public abstract class OrderBaseValidator<T> : AbstractValidator<T>
        where T : OrderBaseDTO
    {
        protected OrderBaseValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.TicketIds)
                .NotNull().WithMessage("TicketIds is required")
                .Must(x => x.Count > 0).WithMessage("At least one ticket must be assigned");
        }
    }

    public class OrderCreateValidator : OrderBaseValidator<OrderCreateDTO> { }

    public class OrderUpdateValidator : OrderBaseValidator<OrderUpdateDTO> { }

    public class OrderPatchValidator : AbstractValidator<OrderPatchDTO>
    {
        public OrderPatchValidator()
        {
            RuleFor(x => x.TicketIds)
            .Must(x => x == null || x.Count > 0)
            .WithMessage("If TicketIds are provided, at least one ticket must be assigned.");

        }
    }
}
