using AnotaAi.Domain.DTOs;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AnotaAi.Domain.Entities;

public class Category
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("title")]
    public string Title { get; set; } = null!;

    [BsonElement("description")]
    public string Description { get; set; } = null!;

    [BsonElement("ownerId")]
    public string OwnerId { get; set; } = null!;

    public Category(CreateCategoryDto categoryDto)
    {
        Title = categoryDto.Title;
        Description = categoryDto.Description;
        OwnerId = categoryDto.OwnerId;
    }
}
