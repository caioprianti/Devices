using FluentValidation;
using Devices.Api.Contracts.Devices;

namespace Devices.Api.Validators.Devices;

public sealed class GetDevicesRequestValidator : AbstractValidator<GetDevicesRequest>
{
    public GetDevicesRequestValidator()
    {
        When(x => x.Brand is not null, () =>
        {
            RuleFor(x => x.Brand)
                .NotEmpty()
                .MaximumLength(100);
        });

        RuleFor(x => x.State)
            .Must(state => !state.HasValue || Enum.IsDefined(state.Value))
            .WithMessage("State must be available, in-use, or inactive.");
    }
}
