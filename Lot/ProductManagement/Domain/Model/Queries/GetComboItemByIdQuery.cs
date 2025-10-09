namespace Lot.ProductManagement.Domain.Model.Queries;

/// <summary>
/// Get Combo Item By Id Query
/// </summary>
/// <param name="Id">
/// El identificador del item de combo a buscar.
/// </param>
public record GetComboItemByIdQuery(int Id);