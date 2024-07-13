namespace AnotaAi.Domain.DTOs;

public record UpdateProductDto
(
    string? Title,
    string? Description,
    decimal? Price,
    string? CategoryId
);