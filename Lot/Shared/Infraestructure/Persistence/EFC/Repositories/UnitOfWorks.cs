using Lot.Shared.Domain.Repositories;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;

namespace Lot.Shared.Infraestructure.Persistence.EFC.Repositories;

/// <summary>
///     Unidad de trabajo para la aplicación.
/// </summary>
public class UnitOfWorks(AppDbContext context) : IUnitOfWork
{
    public async Task CompleteAsync()
    {
        await context.SaveChangesAsync();
    }
}