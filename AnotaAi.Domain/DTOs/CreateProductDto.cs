namespace AnotaAi.Domain.DTOs;

public record CreateProductDto
(
    string Title,
    string Description,
    decimal Price,
    string CategoryId,
    string OwnerId
);