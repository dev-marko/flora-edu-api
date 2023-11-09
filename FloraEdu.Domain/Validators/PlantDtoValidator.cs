using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.Enumerations;
using FluentValidation;

namespace FloraEdu.Domain.Validators;

public class PlantDtoValidator : AbstractValidator<PlantCreateOrUpdateDto>
{
    public PlantDtoValidator()
    {
        RuleFor(plantDto => plantDto.Name)
            .Must(name => name is not null)
            .Must(name => !string.IsNullOrEmpty(name))
            .WithMessage("Please provide a valid name.");
    }
}
