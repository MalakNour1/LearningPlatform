namespace OnlineLearningPlatform.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }

    //Every course belongs to exactly one category
    public ICollection<Course> Courses { get; set; }
        = new List<Course>();// One Category has many Courses
    
    // self-relationship
    // A category can have any number of sub-categories, but each sub-category has exactly one parent.
    public int? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    
    public ICollection<Category> SubCategories { get; set; }
        = new List<Category>();
}