using FluentValidation;
using Devices.Api.Contracts.Devices;

namespace Devices.Api.Validators.Devices;

public sealed class PatchDeviceRequestValidator : AbstractValidator<PatchDeviceRequest>
{
    public PatchDeviceRequestValidator()
    {
        RuleFor(x => x)
            .Must(x => x.Name is not null || x.Brand is not null || x.State.HasValue)
            .WithMessage("At least one property must be provided.");

        When(x => x.Name is not null, () =>
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(200);
        });

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
