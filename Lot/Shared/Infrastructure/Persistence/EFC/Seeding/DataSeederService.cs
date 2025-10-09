using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lot.Shared.Infrastructure.Persistence.EFC.Seeding;

/// <summary>
/// Servicio para inicializar datos de ejemplo en la base de datos
/// </summary>
public static class DataSeederService
{
    /// <summary>
    /// Inicializa los datos de ejemplo en la base de datos
    /// </summary>
    /// <param name="context">El contexto de la base de datos</param>
    public static async Task SeedDataAsync(AppDbContext context)
    {
        Console.WriteLine("📝 Iniciando proceso de seeding paso a paso...");
        await SeedCategoriesAsync(context);
        await SeedUnitsAsync(context);
        await SeedTagsAsync(context);
        // Temporalmente comentamos productos para debuggear las entidades básicas
        await SeedProductsAsync(context);
        Console.WriteLine("🎯 Proceso de seeding completado");
    }

    /// <summary>
    /// Inicializa las categorías de ejemplo
    /// </summary>
    private static async Task SeedCategoriesAsync(AppDbContext context)
    {
        Console.WriteLine("🔍 Verificando si existen categorías...");
        var existingCount = await context.Set<Category>().CountAsync();
        Console.WriteLine($"📊 Categorías existentes: {existingCount}");
        
        if (existingCount > 0) 
        {
            Console.WriteLine("ℹ️ Las categorías ya existen, saltando seeding de categorías");
            return;
        }

        Console.WriteLine("➕ Creando nuevas categorías...");
        
        var categories = new List<Category>
        {
            new Category("Bebidas"),
            new Category("Lácteos"),
            new Category("Panadería"),
            new Category("Carnes"),
            new Category("Frutas y Verduras"),
            new Category("Abarrotes"),
            new Category("Limpieza"),
            new Category("Higiene Personal"),
            new Category("Congelados"),
            new Category("Snacks")
        };
        
        foreach (var categoria in categories)
        {
            Console.WriteLine($"🔸 Categoría: {categoria.Name}");
            await context.Set<Category>().AddAsync(categoria);
        }
        
        Console.WriteLine("💾 Guardando categorías en la base de datos...");
        var changes = await context.SaveChangesAsync();
        Console.WriteLine($"✨ Se guardaron {changes} cambios");
        
        Console.WriteLine("✅ Categorías inicializadas correctamente");
    }

    /// <summary>
    /// Inicializa las unidades de medida de ejemplo
    /// </summary>
    private static async Task SeedUnitsAsync(AppDbContext context)
    {
        Console.WriteLine("🔍 Verificando si existen unidades de medida...");
        var existingCount = await context.Set<Unit>().CountAsync();
        Console.WriteLine($"📊 Unidades existentes: {existingCount}");
        
        if (existingCount > 0) 
        {
            Console.WriteLine("ℹ️ Las unidades de medida ya existen, saltando seeding");
            return;
        }

        Console.WriteLine("➕ Creando nuevas unidades de medida...");
        
        var units = new List<Unit>
        {
            new Unit("Mililitros", "ml"),
            new Unit("Litros", "L"),
            new Unit("Gramos", "g"),
            new Unit("Kilogramos", "kg"),
            new Unit("Unidades", "und"),
            new Unit("Paquetes", "paq"),
            new Unit("Botellas", "bot"),
            new Unit("Latas", "lat"),
            new Unit("Cajas", "caj"),
            new Unit("Docenas", "doc"),
            new Unit("Metros", "m"),
            new Unit("Piezas", "pz")
        };
        
        foreach (var unidad in units)
        {
            Console.WriteLine($"🔸 Unidad: {unidad.Name} ({unidad.Abbreviation})");
            await context.Set<Unit>().AddAsync(unidad);
        }
        
        Console.WriteLine("💾 Guardando unidades en la base de datos...");
        var changes = await context.SaveChangesAsync();
        Console.WriteLine($"✨ Se guardaron {changes} cambios");
        
        Console.WriteLine("✅ Unidades de medida inicializadas correctamente");
    }

    /// <summary>
    /// Inicializa las etiquetas de ejemplo
    /// </summary>
    private static async Task SeedTagsAsync(AppDbContext context)
    {
        Console.WriteLine("🔍 Verificando si existen etiquetas...");
        var existingCount = await context.Set<Tag>().CountAsync();
        Console.WriteLine($"📊 Etiquetas existentes: {existingCount}");
        
        if (existingCount > 0) 
        {
            Console.WriteLine("ℹ️ Las etiquetas ya existen, saltando seeding");
            return;
        }

        Console.WriteLine("➕ Creando nuevas etiquetas...");
        
        var tags = new List<Tag>
        {
            new Tag("Orgánico"),
            new Tag("Sin Gluten"),
            new Tag("Vegano"),
            new Tag("Light"),
            new Tag("Premium"),
            new Tag("Promoción"),
            new Tag("Nuevo"),
            new Tag("Descontinuado"),
            new Tag("Temporada"),
            new Tag("Local"),
            new Tag("Importado"),
            new Tag("Artesanal"),
            new Tag("Sin Azúcar"),
            new Tag("Bajo en Sodio"),
            new Tag("Rica en Fibra")
        };
        
        foreach (var tag in tags)
        {
            Console.WriteLine($"🔸 Etiqueta: {tag.Name}");
            await context.Set<Tag>().AddAsync(tag);
        }
        
        Console.WriteLine("💾 Guardando etiquetas en la base de datos...");
        var changes = await context.SaveChangesAsync();
        Console.WriteLine($"✨ Se guardaron {changes} cambios");
        
        Console.WriteLine("✅ Etiquetas inicializadas correctamente");
    }

    /// <summary>
    /// Inicializa algunos productos de ejemplo
    /// </summary>
    private static async Task SeedProductsAsync(AppDbContext context)
    {
        Console.WriteLine("🔍 Verificando si existen productos...");
        var existingCount = await context.Set<Product>().CountAsync();
        Console.WriteLine($"📊 Productos existentes: {existingCount}");

        if (existingCount > 0)
        {
            Console.WriteLine("ℹ️ Los productos ya existen, saltando seeding de productos");
            return;
        }

        Console.WriteLine("➕ Creando nuevos productos...");

        try
        {
            // Obtener IDs de las entidades relacionadas
            Console.WriteLine("🔗 Buscando categorías para relacionar...");
            var categoria_bebidas = await context.Set<Category>().FirstAsync(c => c.Name == "Bebidas");
            var categoria_lacteos = await context.Set<Category>().FirstAsync(c => c.Name == "Lácteos");
            Console.WriteLine($"🏷️ Categoría Bebidas ID: {categoria_bebidas.Id}");
            Console.WriteLine($"🏷️ Categoría Lácteos ID: {categoria_lacteos.Id}");

            Console.WriteLine("📏 Buscando unidades de medida para relacionar...");
            var unidad_litros = await context.Set<Unit>().FirstAsync(u => u.Abbreviation == "L");
            var unidad_ml = await context.Set<Unit>().FirstAsync(u => u.Abbreviation == "ml");
            Console.WriteLine($"📏 Unidad Litros ID: {unidad_litros.Id}");
            Console.WriteLine($"📏 Unidad ML ID: {unidad_ml.Id}");

            Console.WriteLine("🏷️ Buscando etiquetas para asignar...");
            var tag_premium = await context.Set<Tag>().FirstAsync(t => t.Name == "Premium");
            var tag_promocion = await context.Set<Tag>().FirstAsync(t => t.Name == "Promoción");
            var tag_local = await context.Set<Tag>().FirstAsync(t => t.Name == "Local");
            Console.WriteLine($"🏷️ Tag Premium ID: {tag_premium.Id}");
            Console.WriteLine($"🏷️ Tag Promoción ID: {tag_promocion.Id}");
            Console.WriteLine($"🏷️ Tag Local ID: {tag_local.Id}");

            var productos = new List<Product>
            {
                new Product(
                    "Leche Entera Gloria",
                    "Leche entera pasteurizada, rica en calcio y proteínas",
                    2.50m,
                    3.50m,
                    "Producto de alta rotación, mantener refrigerado",
                    categoria_lacteos.Id,
                    unidad_litros.Id
                ),
                new Product(
                    "Agua San Luis",
                    "Agua mineral natural sin gas, 500ml",
                    0.80m,
                    1.20m,
                    "Producto básico, stock mínimo 100 unidades",
                    categoria_bebidas.Id,
                    unidad_ml.Id
                ),
                new Product(
                    "Coca Cola",
                    "Bebida gaseosa sabor cola, botella de 1L",
                    2.00m,
                    3.00m,
                    "Producto de marca, promoción vigente hasta fin de mes",
                    categoria_bebidas.Id,
                    unidad_litros.Id
                )
            };

            foreach (var producto in productos)
            {
                Console.WriteLine(
                    $"🔸 Producto: {producto.Name} - Precio Compra: ${producto.PurchasePrice} / Precio Venta: ${producto.SalePrice}");
                await context.Set<Product>().AddAsync(producto);
            }

            Console.WriteLine("💾 Guardando productos en la base de datos...");
            var changes = await context.SaveChangesAsync();
            Console.WriteLine($"✨ Se guardaron {changes} cambios de productos");

            // Ahora agregar tags a los productos
            Console.WriteLine("🏷️ Asignando tags a los productos...");
            var productosGuardados = await context.Set<Product>().Where(p =>
                p.Name == "Leche Entera Gloria" ||
                p.Name == "Agua San Luis" ||
                p.Name == "Coca Cola").ToListAsync();

            var productTags = new List<ProductTag>
            {
                new ProductTag(productosGuardados.First(p => p.Name == "Leche Entera Gloria").Id, tag_premium.Id),
                new ProductTag(productosGuardados.First(p => p.Name == "Agua San Luis").Id, tag_local.Id),
                new ProductTag(productosGuardados.First(p => p.Name == "Coca Cola").Id, tag_promocion.Id)
            };

            foreach (var productTag in productTags)
            {
                Console.WriteLine($"🔗 Asignando tag {productTag.TagId} al producto {productTag.ProductId}");
                await context.Set<ProductTag>().AddAsync(productTag);
            }

            Console.WriteLine("💾 Guardando relaciones producto-tag en la base de datos...");
            var tagChanges = await context.SaveChangesAsync();
            Console.WriteLine($"✨ Se guardaron {tagChanges} cambios de tags");

            Console.WriteLine("✅ Productos de ejemplo inicializados correctamente");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error al crear productos: {ex.Message}");
            Console.WriteLine($"🔍 Stack trace: {ex.StackTrace}");
            throw;
        }
    }
    
} 