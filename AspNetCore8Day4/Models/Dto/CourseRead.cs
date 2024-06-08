namespace AspNetCore8Day4.Models.Dto;

public class CourseRead
{
    public int CourseId { get; set; }

    public required string Title { get; set; }

    public int Credits { get; set; }
}