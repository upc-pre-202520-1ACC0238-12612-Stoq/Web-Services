using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Lot.Shared.Infraestructure.ASP.Configuration.Extensions;

/// <summary>
///     Esta clase reemplaza la convención de rutas por defecto por una convención kebab-case.
/// </summary>
public class KebabCaseRouteNamingConvention : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        foreach (var selector in controller.Selectors)
            selector.AttributeRouteModel = ReplaceControllerTemplate(selector, controller.ControllerName);

        foreach (var selector in controller.Actions.SelectMany(a => a.Selectors))
            selector.AttributeRouteModel = ReplaceControllerTemplate(selector, controller.ControllerName);
    }

    private static AttributeRouteModel? ReplaceControllerTemplate(SelectorModel selector, string name)
    {
        return selector.AttributeRouteModel != null
            ? new AttributeRouteModel
            {
                Template = selector.AttributeRouteModel.Template?.Replace("[controller]", ToKebabCase(name))
            }
            : null;
    }

    private static string ToKebabCase(string value)
    {
        // Convierte PascalCase a kebab-case
        return string.Concat(value.Select((x, i) =>
            i > 0 && char.IsUpper(x) ? "-" + x : x.ToString())).ToLower();
    }
}