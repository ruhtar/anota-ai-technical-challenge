using System.Text.Json.Serialization;

namespace AnotaAi.Domain.Entities;

public class Catalog
{
    [JsonPropertyName("owner")]
    public string Owner { get; set; }
    [JsonPropertyName("catalog")]
    public List<CatalogCategory> CatalogItems { get; set; }
}

public class CatalogCategory
{
    [JsonPropertyName("category_title")]
    public string CategoryTitle { get; set; }
    [JsonPropertyName("category_description")]
    public string CategoryDescription { get; set; }
    [JsonPropertyName("itens")]
    public List<Item> Items { get; set; }
}

public class Item
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("price")]
    public decimal Price { get; set; }
}
