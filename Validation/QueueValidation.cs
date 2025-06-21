using Events_system.DTOs;
using FluentValidation;

namespace Events_system.Validation
{
    public abstract class QueueBaseValidator<T> : AbstractValidator<T>
        where T : QueueBaseDTO
    {
        protected QueueBaseValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required");

            RuleFor(x => x.TicketTypeId)
                .GreaterThan(0).WithMessage("TicketTypeId must be a valid ID");
        }
    }

    public class QueueCreateValidator : QueueBaseValidator<QueueCreateDTO> { }

    public class QueueUpdateValidator : QueueBaseValidator<QueueUpdateDTO>
    {
        public QueueUpdateValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value.");
        }
    }

    public class QueuePatchValidator : AbstractValidator<QueuePatchDTO>
    {
        public QueuePatchValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().When(x => x.UserId is not null);

            RuleFor(x => x.TicketTypeId)
                .GreaterThan(0).When(x => x.TicketTypeId.HasValue);

            RuleFor(x => x.Status)
                .IsInEnum().When(x => x.Status.HasValue);
        }
    }
}
