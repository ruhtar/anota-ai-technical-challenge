namespace AnotaAi.Domain.DTOs;

public class ProductCreateDto
{
    public required string Title { get; set; }

    public required string Description { get; set; }

    public decimal Price { get; set; }

    public required string CategoryId { get; set; }

    public required string OwnerId { get; set; }
}
