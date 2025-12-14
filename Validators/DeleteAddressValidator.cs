using Demo.Api.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Demo.Api.Validators;

public class DeleteAddressValidator : AbstractValidator<int>
{
    private readonly ApplicationDbContext _context;

    public DeleteAddressValidator(ApplicationDbContext context)
    {
        _context = context;

        RuleFor(addressId => addressId)
            .MustAsync(CanDeleteAddress)
            .WithMessage("Cannot delete the student's last address. Students must have at least one address.");
    }

    private async Task<bool> CanDeleteAddress(int addressId, CancellationToken cancellationToken)
    {
        var address = await _context.Addresses.FindAsync(new object[] { addressId }, cancellationToken);
        if (address == null)
            return true; // Let controller handle 404

        var studentAddressCount = await _context.Addresses
            .CountAsync(a => a.StudentId == address.StudentId, cancellationToken);

        return studentAddressCount > 1; // Can only delete if student has more than 1 address
    }
}
