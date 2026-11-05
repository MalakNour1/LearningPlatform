using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.Contexts.Configurations;

public class EnrollmentConfiguration
    : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        // Composite Primary Key
        builder.HasKey(e => new { e.StudentId, e.CourseId });

        // Properties
        builder.Property(e => e.EnrollmentDate)
            .IsRequired();

        builder.Property(e => e.ProgressPercentage)
            .IsRequired()
            .HasPrecision(5, 2);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.FinalGrade)
            .IsRequired(false)
            .HasPrecision(5, 2);

        // Student Relationship
        builder.HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId);

        // Course Relationship
        builder.HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId);
        
        builder.ToTable(t =>
            t.HasCheckConstraint(
                "CK_Enrollment_Progress",
                "[ProgressPercentage] >= 0 AND [ProgressPercentage] <= 100"
            )
        );
    }
}