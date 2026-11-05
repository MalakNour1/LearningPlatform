using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;

public class InstructorConfiguration
    : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.FullName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.EmailAddress)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(i => i.ShortBio)
            .IsRequired(false)
            .HasMaxLength(500);

        builder.Property(i => i.DateJoined)
            .IsRequired();
        
    }
}