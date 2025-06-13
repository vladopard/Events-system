using Events_system.DTOs;
using FluentValidation;

namespace Events_system.Validation
{
    public abstract class QueueBaseValidator<T> : AbstractValidator<T>
        where T : QueueBaseDTO
    {
        protected QueueBaseValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.TicketId)
                .GreaterThan(0).WithMessage("TicketId must be a valid ID");
        }
    }

    public class QueueCreateValidator : QueueBaseValidator<QueueCreateDTO> { }

    public class QueueUpdateValidator : QueueBaseValidator<QueueUpdateDTO> { }

    public class QueuePatchValidator : AbstractValidator<QueuePatchDTO>
    {
        public QueuePatchValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0).When(x => x.Quantity.HasValue);

            RuleFor(x => x.UserId)
                .NotEmpty().When(x => x.UserId != null);

            RuleFor(x => x.TicketId)
                .GreaterThan(0).When(x => x.TicketId.HasValue);
        }
    }
}

