using System.ComponentModel.DataAnnotations;

namespace AspNetCore8Day4.Models.Dto;

public class CourseCreate : IValidatableObject
{
    public int CourseId { get; set; }

    [Required(ErrorMessage = "課程名稱為必填欄位")]
    public string Title { get; set; }

    [Required(ErrorMessage = "學分數為必填欄位")]
    [Range(1, 5, ErrorMessage = "學分數必須介於 1 到 5 之間")]
    public int Credits { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (CourseId != 0)
        {
            yield return new ValidationResult("CourseId 不允許指定明確的值", new[] { nameof(CourseId) });
        }

        yield return ValidationResult.Success!;
    }
}