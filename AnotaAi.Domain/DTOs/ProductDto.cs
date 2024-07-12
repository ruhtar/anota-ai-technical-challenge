namespace AnotaAi.Domain.DTOs;

public record ProductDto
(
    string Title,
    string Description,
    decimal Price,
    string CategoryId,
    string OwnerId
);