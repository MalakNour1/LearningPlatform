namespace OnlineLearningPlatform.Models;

public class Student
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string EmailAddress { get; set; }
    public DateTime DateOfBirth { get; set; }
    
   // A student can enroll in many courses, and a course can have many students. 
   public ICollection<Enrollment>? Enrollments { get; set; }
       = new List<Enrollment>();
}