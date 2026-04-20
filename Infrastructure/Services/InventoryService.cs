using System.Runtime.CompilerServices;
using Blazor.Infrastructure.Models;

namespace Blazor.Infrastructure.Services;

public interface IInventoryService
{
    Task<List<Product>> GetProductsAsync();
    IAsyncEnumerable<InventoryUpdate> GetLiveUpdatesAsync(CancellationToken ct = default);
}

public class InventoryService : IInventoryService
{
    private readonly List<Product> _products;

    public InventoryService()
    {
        _products = [
            new Product(Guid.NewGuid(), "EL-NB-001") { Name = "Quantum Laptop", Category = "Electronics", StockLevel = 45, Price = 1299.99m },
            new Product(Guid.NewGuid(), "EL-PH-002") { Name = "SuperNova Smartphone", Category = "Electronics", StockLevel = 120, Price = 899.50m },
            new Product(Guid.NewGuid(), "HO-DS-010") { Name = "ErgoDesk Pro", Category = "Home Office", StockLevel = 12, Price = 450.00m },
            new Product(Guid.NewGuid(), "HO-CH-011") { Name = "AeroChair G3", Category = "Home Office", StockLevel = 8, Price = 299.99m }
        ];
    }

    public async Task<List<Product>> GetProductsAsync()
    {
        // Simulate database latency
        await Task.Delay(1500); 
        return _products;
    }

    /// <summary>
    /// Performance Pattern: Using IAsyncEnumerable for real-time streaming
    /// Allows the UI to update as chunks of data arrive without blocking.
    /// </summary>
    public async IAsyncEnumerable<InventoryUpdate> GetLiveUpdatesAsync([EnumeratorCancellation] CancellationToken ct = default)
    {
        var random = new Random();
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(3000, ct); // Simulate periodic updates

            var product = _products[random.Next(_products.Count)];
            int change = random.Next(-5, 6);
            product.StockLevel = Math.Max(0, product.StockLevel + change);

            yield return new InventoryUpdate(product.Id, product.StockLevel, DateTime.UtcNow);
        }
    }
}
