using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.IAM.Domain.Model.Aggregates;
using Lot.IAM.Application.OutBoundServices;
using Lot.IAM.Infrastructure.Hashing.BCrypt.Services;
using Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Lot.Shared.Infrastructure.Persistence.EFC.Seeding;

/// <summary>
/// Servicio para inicializar datos de ejemplo en la base de datos
/// </summary>
public static class DataSeederService
{
    private static readonly IHashingService HashingService = new HashingService();

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
        await SeedUsersAsync(context);
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
    /// Inicializa usuarios de ejemplo
    /// </summary>
    private static async Task SeedUsersAsync(AppDbContext context)
    {
        Console.WriteLine("🔍 Verificando si existen usuarios...");
        var existingCount = await context.Set<User>().CountAsync();
        Console.WriteLine($"📊 Usuarios existentes: {existingCount}");

        if (existingCount > 0)
        {
            Console.WriteLine("ℹ️ Los usuarios ya existen, saltando seeding de usuarios");
            return;
        }

        Console.WriteLine("➕ Creando nuevos usuarios...");

              var users = new List<User>
        {
            // Usuario Administrator Kevin Chi
            CreateAdminUser("Kevin", "Chi", "Kevin1@gmail.com", "kevin1")
        };

        foreach (var user in users)
        {
            Console.WriteLine($"👤 Usuario: {user.Name} {user.LastName} ({user.Email}) - Rol: {user.Role}");
            await context.Set<User>().AddAsync(user);
        }

        Console.WriteLine("💾 Guardando usuarios en la base de datos...");
        var changes = await context.SaveChangesAsync();
        Console.WriteLine($"✨ Se guardaron {changes} cambios de usuarios");

        Console.WriteLine("✅ Usuarios de ejemplo inicializados correctamente");
    }

    /// <summary>
    /// Crea un usuario administrador para el seeding inicial usando reflexión
    /// </summary>
    private static User CreateAdminUser(string name, string lastName, string email, string password)
    {
        var user = new User();

        // Usar reflexión para establecer propiedades privadas (método para seeding)
        var userType = typeof(User);
        var nameProperty = userType.GetProperty("Name", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        var lastNameProperty = userType.GetProperty("LastName", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        var emailProperty = userType.GetProperty("Email", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        var passwordProperty = userType.GetProperty("Password", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        var roleProperty = userType.GetProperty("Role", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);

        nameProperty?.SetValue(user, name);
        lastNameProperty?.SetValue(user, lastName);
        emailProperty?.SetValue(user, email);
        passwordProperty?.SetValue(user, HashingService.GenerateHash(password));
        roleProperty?.SetValue(user, UserRole.Administrator);

        return user;
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
            var categoria_abarrote = await context.Set<Category>().FirstAsync(c => c.Name == "Abarrotes");
            var categoria_carnes = await context.Set<Category>().FirstAsync(c => c.Name == "Carnes");
            var categoria_frutas_y_verduras = await context.Set<Category>().FirstAsync(c => c.Name == "Frutas y Verduras");
            var categoria_snacks = await context.Set<Category>().FirstAsync(c => c.Name == "Snacks");
            var categoria_limpieza = await context.Set<Category>().FirstAsync(c => c.Name == "Limpieza");

            Console.WriteLine($"🏷️ Categoría Bebidas ID: {categoria_bebidas.Id}");
            Console.WriteLine($"🏷️ Categoría Lácteos ID: {categoria_lacteos.Id}");
            Console.WriteLine($"🏷️ Categoría Abarrotes ID: {categoria_abarrote.Id}");
            Console.WriteLine($"🏷️ Categoría Carnes ID: {categoria_carnes.Id}");
            Console.WriteLine($"🏷️ Categoría Frutas y Verduras ID: {categoria_frutas_y_verduras.Id}");
            Console.WriteLine($"🏷️ Categoría Snacks ID: {categoria_snacks.Id}");
            Console.WriteLine($"🏷️ Categoría Limpieza ID: {categoria_limpieza.Id}");

            Console.WriteLine("📏 Buscando unidades de medida para relacionar...");
            var unidad_litros = await context.Set<Unit>().FirstAsync(u => u.Abbreviation == "L");
            var unidad_ml = await context.Set<Unit>().FirstAsync(u => u.Abbreviation == "ml");
            var unidad_kg = await context.Set<Unit>().FirstAsync(u => u.Abbreviation == "kg");
            var unidad_g = await context.Set<Unit>().FirstAsync(u => u.Abbreviation == "g");
            Console.WriteLine($"📏 Unidad Litros ID: {unidad_litros.Id}");
            Console.WriteLine($"📏 Unidad ML ID: {unidad_ml.Id}");
            Console.WriteLine($"📏 Unidad KG ID: {unidad_kg.Id}");
            Console.WriteLine($"📏 Unidad G ID: {unidad_g.Id}");

            Console.WriteLine("🏷️ Buscando etiquetas para asignar...");
            var tag_premium = await context.Set<Tag>().FirstAsync(t => t.Name == "Premium");
            var tag_promocion = await context.Set<Tag>().FirstAsync(t => t.Name == "Promoción");
            var tag_local = await context.Set<Tag>().FirstAsync(t => t.Name == "Local");
            var tag_importado = await context.Set<Tag>().FirstAsync(t => t.Name == "Importado");
            var tag_artesanal = await context.Set<Tag>().FirstAsync(t => t.Name == "Artesanal");

            Console.WriteLine($"🏷️ Tag Premium ID: {tag_premium.Id}");
            Console.WriteLine($"🏷️ Tag Promoción ID: {tag_promocion.Id}");
            Console.WriteLine($"🏷️ Tag Local ID: {tag_local.Id}");
            Console.WriteLine($"🏷️ Tag Importado ID: {tag_importado.Id}");
            Console.WriteLine($"🏷️ Tag Artesanal ID: {tag_artesanal.Id}");

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
                ),
                // Productos Peruanos
                new Product(
                    "Inca Kola",
                    "Bebida gaseosa peruana, sabor unique, botella de 600ml",
                    1.80m,
                    2.80m,
                    "Bebida nacional peruana, muy popular en mercados locales",
                    categoria_bebidas.Id,
                    unidad_ml.Id
                ),
                new Product(
                    "Harina de Trigo Don Antonio",
                    "Harina de trigo para panadería, grado comercial",
                    4.20m,
                    5.80m,
                    "Harina de calidad para panificación artesanal e industrial",
                    categoria_abarrote.Id,
                    unidad_kg.Id
                ),
                new Product(
                    "Arroz Costeño Tumi",
                    "Arroz blanco grano largo, presentación de 5kg",
                    18.50m,
                    25.00m,
                    "Arroz de consumo diario, grano premium peruano",
                    categoria_abarrote.Id,
                    unidad_kg.Id
                ),
                new Product(
                    "Pollo Fresco Granja San Fernando",
                    "Pollo entero fresco, aproximadamente 2.5kg",
                    8.50m,
                    12.00m,
                    "Pollo criollo peruano, carne tierna y sabrosa",
                    categoria_carnes.Id,
                    unidad_kg.Id
                ),
                new Product(
                    "Queso Fresco Andino",
                    "Queso fresco artesanal, presentación de 500g",
                    12.00m,
                    18.00m,
                    "Queso tradicional andino, textura cremosa",
                    categoria_lacteos.Id,
                    unidad_g.Id
                ),
                new Product(
                    "Aji Amarillo Peruano",
                    "Aji amarillo seco, presentación de 200g",
                    6.80m,
                    10.50m,
                    "Aji fundamental en la gastronomía peruana, nivel de picante medio",
                    categoria_frutas_y_verduras.Id,
                    unidad_g.Id
                ),
                new Product(
                    "Papa Huayro Nativa",
                    "Papa nativa peruana, presentación de 1kg",
                    4.50m,
                    7.00m,
                    "Variedad andina, ideal para causas y guisos tradicionales",
                    categoria_frutas_y_verduras.Id,
                    unidad_kg.Id
                ),
                new Product(
                    "Cancha Serrana Taki",
                    "Cancha de maíz tostada, bolsa de 200g",
                    2.20m,
                    4.00m,
                    "Snack tradicional peruano, crujiente y saludable",
                    categoria_snacks.Id,
                    unidad_g.Id
                ),
                new Product(
                    "Lavalozas Líquido Limón",
                    "Limpiador líquido con aroma a limón, 750ml",
                    4.80m,
                    6.50m,
                    "Limpieza multiusos, aroma cítrico peruano",
                    categoria_limpieza.Id,
                    unidad_ml.Id
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
            var productosGuardados = await context.Set<Product>().ToListAsync();

            var productTags = new List<ProductTag>
            {
                // Tags para productos originales
                new ProductTag(productosGuardados.First(p => p.Name == "Leche Entera Gloria").Id, tag_premium.Id),
                new ProductTag(productosGuardados.First(p => p.Name == "Agua San Luis").Id, tag_local.Id),
                new ProductTag(productosGuardados.First(p => p.Name == "Coca Cola").Id, tag_promocion.Id),
                // Tags para productos peruanos
                new ProductTag(productosGuardados.First(p => p.Name == "Inca Kola").Id, tag_local.Id),
                new ProductTag(productosGuardados.First(p => p.Name == "Arroz Costeño Tumi").Id, tag_premium.Id),
                new ProductTag(productosGuardados.First(p => p.Name == "Pollo Fresco Granja San Fernando").Id, tag_local.Id),
                new ProductTag(productosGuardados.First(p => p.Name == "Queso Fresco Andino").Id, tag_artesanal.Id),
                new ProductTag(productosGuardados.First(p => p.Name == "Papa Huayro Nativa").Id, tag_importado.Id),
                new ProductTag(productosGuardados.First(p => p.Name == "Cancha Serrana Taki").Id, tag_artesanal.Id),
                new ProductTag(productosGuardados.First(p => p.Name == "Lavalozas Líquido Limón").Id, tag_local.Id)
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