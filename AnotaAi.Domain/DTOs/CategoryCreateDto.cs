namespace AnotaAi.Domain.DTOs;

public class CategoryCreateDto
{
    public required string Title { get; set; }

    public required string Description { get; set; }

    public required string OwnerId { get; set; }
}
