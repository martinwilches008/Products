using Products.Domain.Entities;

namespace Products.Application.Ports;

public interface IProductRepository
{
    // Buscar un producto por su identificador único
    Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken ct = default);

    // Añadir un nuevo producto
    Task AddAsync(
        Product product,
        CancellationToken ct = default);

    // Obtener productos paginados
    Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken ct = default);

    // Persistir los cambios pendientes
    Task SaveChangesAsync(CancellationToken ct = default);
}