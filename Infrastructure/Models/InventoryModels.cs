using System.ComponentModel.DataAnnotations;

namespace Blazor.Infrastructure.Models;

public class Product(Guid id, string sku)
{
    public Guid Id { get; init; } = id;
    
    [Required]
    public string SKU { get; init; } = sku;

    [Required]
    public string Name { get; set; } = string.Empty;

    // 'field' keyword: Property-scoped field
    // Removes the need for a private backing field '_category'
    public string Category 
    { 
        get => field ?? "Uncategorized"; 
        set => field = value?.Trim(); 
    }

    public int StockLevel { get; set; }

    public decimal Price 
    { 
        get; 
        set => field = value < 0 ? 0 : value; 
    }
}

public record InventoryUpdate(Guid ProductId, int NewLevel, DateTime UpdatedAt);

public enum StockStatus
{
    InStock,
    LowStock,
    OutOfStock
}
