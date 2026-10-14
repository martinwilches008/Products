namespace Products.API.Models;

// Datos necesarios para crear un producto
public record CreateProductRequest(
    string Name,
    decimal Price,
    int InitialStock,
    string Description
);

// Datos necesarios para actualizar el stock de un producto
public record AdjustStockRequest(int Quantity);