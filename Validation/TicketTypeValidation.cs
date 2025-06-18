using Events_system.DTOs;
using FluentValidation;

namespace Events_system.Validation
{
    public abstract class TicketTypeBaseValidator<T> : AbstractValidator<T>
        where T : TicketTypeBaseDTO
    {
        protected TicketTypeBaseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than zero");

            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("EventId is required");
        }
    }

    public class TicketTypeCreateValidator : TicketTypeBaseValidator<TicketTypeCreateDTO> { }

    public class TicketTypeUpdateValidator : TicketTypeBaseValidator<TicketTypeUpdateDTO> { }

    public class TicketTypePatchValidator : AbstractValidator<TicketTypePatchDTO>
    {
        public TicketTypePatchValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100);

            RuleFor(x => x.Price)
                .GreaterThan(0).When(x => x.Price.HasValue);

            RuleFor(x => x.EventId)
                .GreaterThan(0).When(x => x.EventId.HasValue);
        }
    }
}
