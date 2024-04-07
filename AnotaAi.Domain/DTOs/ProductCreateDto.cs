namespace AnotaAi.Domain.DTOs
{
    public class ProductCreateDto
    {
        public string Title { get; set; } = null!;

        public string Description { get; set; } = null!;

        public decimal Price { get; set; }

        public string CategoryId { get; set; } = null!;

        public string OwnerId { get; set; } = null!;
    }
}
