using AddressBook.Shared.Models.Requests;
using FluentValidation;

namespace AddressBook.BusinessLayer.Validations;

public class SavePersonRequestValidator : AbstractValidator<SavePersonRequest>
{
    public SavePersonRequestValidator()
    {
        RuleFor(p => p.FirstName)
            .MaximumLength(256)
            .NotEmpty()
            .WithMessage("the first name is required");

        RuleFor(p => p.LastName)
            .MaximumLength(256)
            .NotEmpty()
            .WithMessage("the last name is required");

        RuleFor(p => p.BirthDate)
            .NotEmpty()
            .WithMessage("the date of birth is required");

        RuleFor(p => p.Gender)
            .MaximumLength(10)
            .NotEmpty()
            .WithMessage("the gender is required");

        RuleFor(p => p.City)
            .MaximumLength(50)
            .NotEmpty()
            .WithMessage("the city is required");

        RuleFor(p => p.Province)
            .MaximumLength(10)
            .NotEmpty()
            .WithMessage("the province is required");

        RuleFor(p => p.CellphoneNumber).MaximumLength(50);
        RuleFor(p => p.EmailAddress).MaximumLength(256);
    }
}