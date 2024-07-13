namespace AnotaAi.Domain.Entities;

public class Catalog
{
    public string Owner { get; set; }
    public List<Category> CatalogItems { get; set; }
}

public class CatalogCategory
{
    public string CategoryTitle { get; set; }
    public string CategoryDescription { get; set; }
    public List<Item> Items { get; set; }
}

public class Item
{
    public string Title { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
}
