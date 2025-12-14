using Demo.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Demo.Api.Data.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.AddressId);
        
        builder.Property(a => a.Street)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(a => a.Province)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(a => a.PostalCode)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(100);

        // Relationship: Each Address belongs to one Student => ON DELETE CASCADE
        // When Student is deleted, all its Addresses are automatically deleted (cascade)
        builder.HasOne(a => a.Student)
            .WithMany(s => s.Addresses)
            .HasForeignKey(a => a.StudentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
