namespace AnotaAi.Domain.DTOs
{
    public class CategoryCreateDto
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string OwnerId { get; set; } = null!;
    }
}
