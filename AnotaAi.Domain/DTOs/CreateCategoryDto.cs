namespace AnotaAi.Domain.DTOs;

public record CreateCategoryDto
(
    string Title,
    string Description,
    string OwnerId
);