namespace OnlineLearningPlatform.Models;
/*
A student can enroll in many courses, and a course can have many students.
A student can enroll in the same course only once. 
*/

public enum EnrollmentStatus
{
    Active,
    Completed,
    Dropped
}
public class Enrollment
{
    public int CourseId { get; set; }
    public int StudentId { get; set; }

    public DateTime EnrollmentDate { get; set; }
    public decimal ProgressPercentage { get; set; }
    public EnrollmentStatus Status { get; set; }
    public decimal? FinalGrade { get; set; }

    public Student Student { get; set; }
    public Course Course { get; set; }
}