namespace OnlineLearningPlatform.Models;

public enum Level
{
    Beginner,
    Intermediate,
    Advanced
}
public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public Level Level { get; set; }
    public bool IsPublished { get; set; }
    
    //The published date is recorded only when the course gets published, so it is empty for unpublished courses.
    public DateTime? DatePublished { get; set; }
    
    //An instructor can publish many courses, but every course is published by exactly one instructor.
    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; }
    
    //Every course belongs to exactly one category
    public int CourseCategoryId { get; set; }
    public Category CourseCategory { get; set; }
    
    //A student can enroll in many courses, and (a course can have many students).
    public ICollection<Enrollment>? Enrollments { get; set; }
        = new List<Enrollment>();
}