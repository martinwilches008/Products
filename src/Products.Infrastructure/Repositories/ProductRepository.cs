using Microsoft.EntityFrameworkCore;
using Products.Application.Ports;
using Products.Domain.Entities;
using Products.Infrastructure.Persistence;

namespace Products.Infrastructure.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _context;

    public ProductRepository(AppDbContext context) => _context = context;

    // Buscar un producto por su identificador
    public async Task<Product?> GetByIdAsync(
        Guid id,
        CancellationToken ct = default)
        => await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id, ct);

    // Agregar un nuevo producto
    public async Task AddAsync(
        Product product,
        CancellationToken ct = default)
        => await _context.Products
            .AddAsync(product, ct);

    // Obtener productos paginados junto al total de productos
    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken ct = default)
    {
        var query = _context.Products.AsNoTracking();

        var totalCount = await query.CountAsync(ct);

        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    // Guardar en base de datos
    public async Task SaveChangesAsync(
        CancellationToken ct = default)
        => await _context
            .SaveChangesAsync(ct);
}