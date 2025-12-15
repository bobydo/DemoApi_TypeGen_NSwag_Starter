using Demo.Api.Models;
using FluentValidation;

namespace Demo.Api.Validators;

public class CreateStudentRequestValidator : AbstractValidator<CreateStudentRequest>
{
    /// <summary>
    /// FluentValidation's automatic validation applies to request body objects (like CreateStudentRequest in POST/PUT)
    /// so validation happens before your controller action executes.
    /// </summary>
    public CreateStudentRequestValidator()
    {
        RuleFor(x => x.StudentNo)
            .NotEmpty().WithMessage("Student number is required")
            .MaximumLength(8).WithMessage("Student number cannot exceed 8 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name cannot exceed 100 characters");

        RuleFor(x => x.Addresses)
            .NotNull().WithMessage("Addresses are required")
            .Must(addresses => addresses != null && addresses.Count > 0)
            .WithMessage("At least one address is required");

        RuleForEach(x => x.Addresses).ChildRules(address =>
        {
            address.RuleFor(a => a.Street)
                .NotEmpty().WithMessage("Street is required")
                .MaximumLength(50).WithMessage("Street cannot exceed 50 characters");

            address.RuleFor(a => a.City)
                .NotEmpty().WithMessage("City is required")
                .MaximumLength(40).WithMessage("City cannot exceed 40 characters");

            address.RuleFor(a => a.Province)
                .NotEmpty().WithMessage("Province is required")
                .MaximumLength(30).WithMessage("Province cannot exceed 30 characters");

            address.RuleFor(a => a.PostalCode)
                .NotEmpty().WithMessage("Postal code is required")
                .MaximumLength(7).WithMessage("Postal code cannot exceed 7 characters");

            address.RuleFor(a => a.Country)
                .NotEmpty().WithMessage("Country is required")
                .MaximumLength(30).WithMessage("Country cannot exceed 30 characters");
        });
    }
}
