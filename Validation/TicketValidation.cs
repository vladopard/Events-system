using Events_system.DTOs;
using FluentValidation;

namespace Events_system.Validation
{
    public abstract class TicketBaseValidator<T> : AbstractValidator<T>
        where T : TicketBaseDTO
    {
        protected TicketBaseValidator()
        {
            RuleFor(x => x.Seat)
                .NotEmpty().WithMessage("Seat is required")
                .MaximumLength(20);

            RuleFor(x => x.EventId)
                .GreaterThan(0).WithMessage("EventId must be a valid ID");

            RuleFor(x => x.TicketTypeId)
                .GreaterThan(0).WithMessage("TicketTypeId must be a valid ID");
        }
    }

    public class TicketCreateValidator : TicketBaseValidator<TicketCreateDTO> { }

    public class TicketUpdateValidator : TicketBaseValidator<TicketUpdateDTO> { }

    public class TicketPatchValidator : AbstractValidator<TicketPatchDTO>
    {
        public TicketPatchValidator()
        {
            RuleFor(x => x.Seat)
                .MaximumLength(20);

            RuleFor(x => x.EventId)
                .GreaterThan(0).When(x => x.EventId.HasValue);

            RuleFor(x => x.TicketTypeId)
                .GreaterThan(0).When(x => x.TicketTypeId.HasValue);
            RuleFor(x => x.OrderId)
                .GreaterThan(0).When(x => x.OrderId.HasValue); 
        }
    }
}
