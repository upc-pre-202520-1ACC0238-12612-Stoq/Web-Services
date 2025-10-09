namespace Lot.ProductManagement.Domain.Model.Queries;

/// <summary>
/// Get Combo Items By Combo Id Query
/// </summary>
/// <param name="ComboId">
/// El identificador del combo cuyos items se desean buscar.
/// </param>
public record GetComboItemsByComboIdQuery(int ComboId);