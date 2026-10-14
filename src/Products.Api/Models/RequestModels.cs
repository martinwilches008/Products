namespace Products.API.Models;

// Datos necesarios para crear un producto
public class CreateProductRequest
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int InitialStock { get; init; }
}

// Datos necesarios para actualizar el stock de un producto
public class AdjustStockRequest
{
    public int Quantity { get; init; }
}