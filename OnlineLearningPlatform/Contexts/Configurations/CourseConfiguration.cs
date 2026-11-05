using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform.Contexts.Configurations;

public class CourseConfiguration
    : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Description)
            .IsRequired(false);

        builder.Property(c => c.Price)
            .HasPrecision(10, 2);

        builder.Property(c => c.Level)
            .IsRequired();

        builder.Property(c => c.IsPublished)
            .IsRequired();

        builder.Property(c => c.DatePublished)
            .IsRequired(false);

        // Instructor Relationship
        builder.HasOne(c => c.Instructor)
            .WithMany(i => i.Courses)
            .HasForeignKey(c => c.InstructorId);

        // Category Relationship
        builder.HasOne(c => c.CourseCategory)
            .WithMany(cat => cat.Courses)
            .HasForeignKey(c => c.CourseCategoryId);
    }
}