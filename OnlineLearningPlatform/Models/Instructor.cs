namespace OnlineLearningPlatform.Models;

public class Instructor
{
    public  int Id { get; set; }
    public string FullName { get; set; }
    public string EmailAddress { get; set; }
    public string? ShortBio { get; set; }
    public DateTime DateJoined { get; set; }
    
    //An instructor can publish many courses, but every course is published by exactly one instructor.
    public ICollection<Course> Courses { get; set; }
}
