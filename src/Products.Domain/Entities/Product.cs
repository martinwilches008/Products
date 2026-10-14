namespace Products.Domain.Entities;

public class Product
{
    // Esquema del producto
    public Guid Id { get; private set; }
    public int Stock { get; private set; }
    public decimal Price { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;

    private Product() {}

    public Product(string name, string description, decimal price, int initialStock)
    {
        // Se valida que el nombre del producto no sea nulo o un espacio vacío
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre es obligatorio.");
        // Se valida que el precio del producto sea mayor a 0
        if (price <= 0)
            throw new ArgumentException("El precio debe ser mayor a cero.");
        // Se valida que el stock inicial sea mayor a 0
        if (initialStock < 0)
            throw new ArgumentException("El stock inicial no puede ser negativo.");

        Id      = Guid.NewGuid();
        Name    = name;
        Price   = price;
        Stock   = initialStock;
        Description = description;
    }

    public void AdjustStock(int quantity)
    {
        // Al modificar el stock, validar que este no quede con valores negativos
        if (Stock + quantity < 0)
            throw new InvalidOperationException("El stock no puede quedar en negativo.");
        Stock += quantity;
    }
}