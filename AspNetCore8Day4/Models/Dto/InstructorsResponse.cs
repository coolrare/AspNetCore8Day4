namespace AspNetCore8Day4.Models.Dto;

public class InstructorsResponse
{
    public int Id { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? Discriminator { get; set; }
}
