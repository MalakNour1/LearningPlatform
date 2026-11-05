using Microsoft.EntityFrameworkCore;
using OnlineLearningPlatform.Contexts;
using OnlineLearningPlatform.Models;

namespace OnlineLearningPlatform;

internal class Program
{
    private static void Main(string[] args)
    {
        // AddInstructor(); // id = 1
        // AddCategory(); // id = 1
        // AddSubCategory(1); // id = 2
        // AddCourse(); // id = 1
        // AddStudents(); // ids = 1,2,3
        // PublishCourse(1);
        // EnrollStudent(1, 1);
        // EnrollStudent(2, 1);
        // EnrollStudent(3, 1);

        // ChangeCoursePrice(1, 600);
        // UpdateProgress(1, 1, 50);

        // UnenrollStudent(1, 1);
        /*
        TryDeleteCategory(1); 
        Cannot delete this category.
        Database message: An error occurred while saving the entity changes. See the inner exception for detail
       */
        //DeleteCategorySafely(1); //Cannot delete category because it has sub-categories.

        DeleteCourse(1);
        /*
        Enrollments before deletion: 2
        Enrollments after deletion: 0
        Course deleted successfully.
         */
    }

    //Create
    private static void AddInstructor()
    {
        using var context = new PlatformDbContext();

        var instructor = new Instructor
        {
            FullName = "Ahmed Mohamed",
            EmailAddress = "ahmed@example.com",
            ShortBio = "Backend Development Instructor",
            DateJoined = DateTime.Now
        };

        context.Instructors.Add(instructor);
        context.SaveChanges();

        Console.WriteLine($"Instructor added successfully. ID: {instructor.Id}");
    }

    private static void AddCategory()
    {
        using var context = new PlatformDbContext();

        var category = new Category
        {
            Name = "Programming",
            Description = "Programming courses",
            ParentCategoryId = null
        };

        context.Categories.Add(category);

        context.SaveChanges();

        Console.WriteLine($"Category added successfully. ID: {category.Id}");
    }

    private static void AddSubCategory(int parentCategoryId)
    {
        using var context = new PlatformDbContext();

        var parentExists = context.Categories
            .Any(c => c.Id == parentCategoryId);

        if (!parentExists)
        {
            Console.WriteLine("Parent category not found.");
            return;
        }

        var category = new Category
        {
            Name = "Backend",
            Description = "Backend development courses",
            ParentCategoryId = parentCategoryId
        };

        context.Categories.Add(category);

        context.SaveChanges();

        Console.WriteLine($"Sub-category added. ID: {category.Id}");
    }

    private static void AddCourse()
    {
        using var context = new PlatformDbContext();

        var instructorId = 1;
        var categoryId = 2;

        var instructorExists = context.Instructors
            .Any(i => i.Id == instructorId);

        var categoryExists = context.Categories
            .Any(c => c.Id == categoryId);

        if (!instructorExists)
        {
            Console.WriteLine("Instructor not found.");
            return;
        }

        if (!categoryExists)
        {
            Console.WriteLine("Category not found.");
            return;
        }

        var course = new Course
        {
            Title = "C# Fundamentals",
            Description = "Learn C# from scratch",
            Price = 500,
            Level = Level.Beginner,
            IsPublished = false,
            DatePublished = null,
            InstructorId = instructorId,
            CourseCategoryId = categoryId
        };

        context.Courses.Add(course);
        context.SaveChanges();

        Console.WriteLine($"Course added successfully. ID: {course.Id}");
    }

    private static void AddStudents()
    {
        using var dbContext = new PlatformDbContext();

        var students = new List<Student>
        {
            new()
            {
                FullName = "Malak Nour",
                DateOfBirth = new DateTime(2005, 12, 5),
                EmailAddress = "kokyn77@gmail.com"
            },
            new()
            {
                FullName = "M.Mohy",
                DateOfBirth = new DateTime(2005, 8, 11),
                EmailAddress = "monmon8@gmail.com"
            },
            new()
            {
                FullName = "B.Mamdouh",
                DateOfBirth = new DateTime(2005, 12, 3),
                EmailAddress = "basbosty12@gmail.com"
            }
        };

        dbContext.Students.AddRange(students);
        dbContext.SaveChanges();
        foreach (var student in students)
            Console.WriteLine(
                $"Student added successfully. ID: {student.Id}");
    }

    private static void EnrollStudent(int studentId, int courseId)
    {
        using var context = new PlatformDbContext();

        var studentExists = context.Students
            .Any(s => s.Id == studentId);

        if (!studentExists)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        var course = context.Courses
            .Find(courseId);

        if (course == null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        if (!course.IsPublished)
        {
            Console.WriteLine("Cannot enroll in an unpublished course.");
            return;
        }

        var alreadyEnrolled = context.Enrollments
            .Any(e => e.StudentId == studentId &&
                      e.CourseId == courseId);

        if (alreadyEnrolled)
        {
            Console.WriteLine("Student is already enrolled.");
            return;
        }

        var enrollment = new Enrollment
        {
            StudentId = studentId,
            CourseId = courseId,
            EnrollmentDate = DateTime.Now,
            ProgressPercentage = 0,
            Status = EnrollmentStatus.Active,
            FinalGrade = null
        };

        context.Enrollments.Add(enrollment);

        context.SaveChanges();

        Console.WriteLine("Enrollment added successfully.");
    }

    //Read
    private static void ReadAllInstructors()
    {
        using var dbContext = new PlatformDbContext();
        var instructors = dbContext.Instructors.ToList();
        if (instructors.Count == 0)
        {
            Console.WriteLine("No instructors found.");
            return;
        }

        foreach (var instructor in instructors)
            Console.WriteLine(
                $"ID: {instructor.Id}, " +
                $"Name: {instructor.FullName}, " +
                $"Email: {instructor.EmailAddress}");
    }

    private static void FindCourseById(int courseId)
    {
        using var dbContext = new PlatformDbContext();

        var courseById = dbContext.Courses.Find(courseId);

        if (courseById == null)
            Console.WriteLine($"Course with ID {courseId} not found.");
        else
            Console.WriteLine(
                $"Found by ID: {courseById.Title}, Price: {courseById.Price}");
    }

    private static void FindCourseByTitle(string courseTitle)
    {
        using var dbContext = new PlatformDbContext();

        var courseByTitle = dbContext.Courses
            .FirstOrDefault(c => c.Title == courseTitle);

        if (courseByTitle == null)
            Console.WriteLine($"Course with title '{courseTitle}' not found.");
        else
            Console.WriteLine(
                $"Found by Title: ID: {courseByTitle.Id}, Price: {courseByTitle.Price}");
    }

    private static void FilterCourses(decimal minPrice, decimal maxPrice)
    {
        using var dbContext = new PlatformDbContext();

        var courses = dbContext.Courses
            .Where(c => c.Price >= minPrice && c.Price <= maxPrice)
            .OrderByDescending(c => c.Price)
            .ToList();

        if (courses.Count == 0)
        {
            Console.WriteLine("No courses found in this price range.");
            return;
        }

        foreach (var course in courses)
            Console.WriteLine(
                $"Title: {course.Title}, Level: {course.Level}, Price: {course.Price}");
    }

    private static void ReadCategories(int categoryId)
    {
        //Print the direct sub-categories of a given category, then print the top-level categories.
        using var dbContext = new PlatformDbContext();

        // Direct sub-categories
        var subCategories = dbContext.Categories
            .Where(c => c.ParentCategoryId == categoryId)
            .ToList();

        Console.WriteLine("Sub-categories:");

        if (subCategories.Count == 0)
            Console.WriteLine("No sub-categories found.");
        else
            foreach (var category in subCategories)
                Console.WriteLine($"ID: {category.Id}, Name: {category.Name}");

        // Top-level categories
        var topLevelCategories = dbContext.Categories
            .Where(c => c.ParentCategoryId == null)
            .ToList();

        Console.WriteLine("\nTop-level categories:");

        if (topLevelCategories.Count == 0)
            Console.WriteLine("No top-level categories found.");
        else
            foreach (var category in topLevelCategories)
                Console.WriteLine($"ID: {category.Id}, Name: {category.Name}");
    }

    private static void ReadCourseRoster(int courseId)
    {
        using var dbContext = new PlatformDbContext();

        var course = dbContext.Courses.Find(courseId);

        if (course == null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        // Get enrolled student IDs first
        var studentIds = dbContext.Enrollments
            .Where(e => e.CourseId == courseId)
            .Select(e => e.StudentId)
            .ToList();

        Console.WriteLine($"Course: {course.Title}");
        Console.WriteLine($"Number of enrolled students: {studentIds.Count}");

        if (studentIds.Count == 0)
        {
            Console.WriteLine("No students enrolled in this course.");
            return;
        }

        // Get students using Contains
        var students = dbContext.Students
            .Where(s => studentIds.Contains(s.Id))
            .ToList();

        foreach (var student in students) Console.WriteLine(student.FullName);
    }

    //Update
    private static void ChangeCoursePrice(int courseId, decimal newPrice)
    {
        using var context = new PlatformDbContext();

        var course = context.Courses.Find(courseId);

        if (course == null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        if (newPrice < 0)
        {
            Console.WriteLine("Price cannot be negative.");
            return;
        }

        course.Price = newPrice;

        context.SaveChanges();

        Console.WriteLine($"Course price updated successfully. New price: {course.Price}");
    }

    private static void UpdateProgress(int studentId, int courseId, int newProgress)
    {
        using var context = new PlatformDbContext();

        if (newProgress < 0 || newProgress > 100)
        {
            Console.WriteLine("Progress must be between 0 and 100.");
            return;
        }

        var enrollment = context.Enrollments.Find(studentId, courseId);

        if (enrollment == null)
        {
            Console.WriteLine("Enrollment not found.");
            return;
        }

        enrollment.ProgressPercentage = newProgress;

        if (newProgress == 100) enrollment.Status = EnrollmentStatus.Completed;

        context.SaveChanges();

        Console.WriteLine($"Progress updated successfully: {enrollment.ProgressPercentage}%");
    }

    private static void PublishCourse(int courseId)
    {
        using var dbContext = new PlatformDbContext();

        var course = dbContext.Courses.Find(courseId);
        if (course == null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        if (course.IsPublished)
        {
            Console.WriteLine("Course is already published.");
            return;
        }

        course.IsPublished = true;
        course.DatePublished = DateTime.Now;
        dbContext.SaveChanges();
        Console.WriteLine($"Course published successfully. ID: {course.Id}");
    }

    //Delete
    private static void UnenrollStudent(int studentId, int courseId)
    {
        using var context = new PlatformDbContext();

        var enrollment = context.Enrollments.Find(studentId, courseId);

        if (enrollment == null)
        {
            Console.WriteLine("Enrollment not found.");
            return;
        }

        context.Enrollments.Remove(enrollment);
        context.SaveChanges();

        Console.WriteLine("Student unenrolled successfully.");
    }

    private static void TryDeleteCategory(int categoryId)
    {
        using var context = new PlatformDbContext();
        var category = context.Categories.Find(categoryId);
        if (category == null)
        {
            Console.WriteLine("Category not found.");
            return;
        }

        try
        {
            context.Categories.Remove(category);
            context.SaveChanges();

            Console.WriteLine("Category deleted successfully.");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("Cannot delete this category.");
            Console.WriteLine($"Database message: {ex.Message}");
        }
    }

    private static void DeleteCategorySafely(int categoryId)
    {
        using var dbContext = new PlatformDbContext();

        var category = dbContext.Categories.Find(categoryId);

        if (category == null)
        {
            Console.WriteLine("Category not found.");
            return;
        }

        var hasSubCategories = dbContext.Categories
            .Any(c => c.ParentCategoryId == categoryId);

        if (hasSubCategories)
        {
            Console.WriteLine("Cannot delete category because it has sub-categories.");
            return;
        }

        dbContext.Categories.Remove(category);
        dbContext.SaveChanges();

        Console.WriteLine("Category deleted successfully.");
    }

    private static void DeleteCourse(int courseId)
    {
        using var dbContext = new PlatformDbContext();

        var course = dbContext.Courses.Find(courseId);

        if (course == null)
        {
            Console.WriteLine("Course not found.");
            return;
        }

        var enrollmentsBefore = dbContext.Enrollments
            .Count(e => e.CourseId == courseId);

        Console.WriteLine($"Enrollments before deletion: {enrollmentsBefore}");

        // Enrollments are deleted automatically because of Cascade Delete.
        dbContext.Courses.Remove(course);
        dbContext.SaveChanges();

        var enrollmentsAfter = dbContext.Enrollments
            .Count(e => e.CourseId == courseId);

        Console.WriteLine($"Enrollments after deletion: {enrollmentsAfter}");
        Console.WriteLine("Course deleted successfully.");
    }
}