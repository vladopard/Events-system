using Events_system.DTOs;
using FluentValidation;

namespace Events_system.Validation
{
    public abstract class EventValidation<T> : AbstractValidator<T>
        where T : EventBaseDTO
    {
        protected EventValidation()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Event name is required")
                .MaximumLength(100);

            RuleFor(x => x.Venue)
                .NotEmpty().WithMessage("Venue is required")
                .MaximumLength(200);

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(100);

            RuleFor(x => x.StartDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("Start date must be in the future");
        }
    }

    public class EventCreateValidator : EventValidation<EventCreateDTO>
    {
        public EventCreateValidator() : base() { }
    }

    public class EventUpdateValidator : EventValidation<EventUpdateDTO>
    {
        public EventUpdateValidator() : base() { }
    }

    public class EventPatchValidator : AbstractValidator<EventPatchDTO>
    {
        public EventPatchValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100);

            RuleFor(x => x.Venue)
                .MaximumLength(200);

            RuleFor(x => x.City)
                .MaximumLength(100);

            RuleFor(x => x.StartDate)
                .Must(d => d == null || d > DateTime.UtcNow)
                .WithMessage("Start date must be in the future");
        }
    }
}
