using Lot.IAM.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.Reports.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;
using Lot.Inventaries.Domain.Model.Aggregates;



namespace Lot.Shared.Infraestructure.Persistence.EFC.Configuration.Extensions;


/// <summary>
///     Contexto de base de datos para la aplicación Lot
/// </summary>
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    // En el método OnModelCreating
    protected override void OnConfiguring(DbContextOptionsBuilder builder)
    {
        base.OnConfiguring(builder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<User>().HasKey(u => u.Id);
        builder.Entity<User>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<User>().Property(u => u.Name).IsRequired();
        builder.Entity<User>().Property(u => u.LastName).IsRequired();
        builder.Entity<User>().Property(u => u.Password).IsRequired();
        builder.Entity<User>().Property(u => u.Email).IsRequired();

        // Configuración de Inventory
        //by-product
        builder.Entity<InventoryByProduct>().HasKey(p => p.Id);

        builder.Entity<InventoryByProduct>().Property(p => p.Id)
            .IsRequired().ValueGeneratedOnAdd();

        builder.Entity<InventoryByProduct>().Property(p => p.Categoria)
            .HasColumnName("Categoria").IsRequired().HasMaxLength(100);

        builder.Entity<InventoryByProduct>().Property(p => p.Producto)
            .HasColumnName("Producto").IsRequired().HasMaxLength(100);

        builder.Entity<InventoryByProduct>().Property(p => p.FechaEntrada)
            .HasColumnName("FechaEntrada").HasColumnType("timestamp(6)");

        builder.Entity<InventoryByProduct>().Property(p => p.Cantidad)
            .HasColumnName("Cantidad");

        builder.Entity<InventoryByProduct>().Property(p => p.Precio)
            .HasColumnName("Precio").HasColumnType("decimal(10,2)");

        builder.Entity<InventoryByProduct>().Property(p => p.StockMinimo)
            .HasColumnName("StockMinimo");

        builder.Entity<InventoryByProduct>().Property(p => p.UnidadMedida)
            .HasColumnName("UnidadMedida").HasMaxLength(50);

        builder.Entity<InventoryByProduct>().ToTable("inventory_by_product");

        //by-bach
        builder.Entity<InventoryByBatch>().HasKey(b => b.Id);

        builder.Entity<InventoryByBatch>().Property(b => b.Id)
            .IsRequired().ValueGeneratedOnAdd();

        builder.Entity<InventoryByBatch>().Property(b => b.Proveedor)
            .HasColumnName("Proveedor").IsRequired().HasMaxLength(100);

        builder.Entity<InventoryByBatch>().Property(b => b.Producto)
            .HasColumnName("Producto").IsRequired().HasMaxLength(100);

        builder.Entity<InventoryByBatch>().Property(b => b.FechaEntrada)
            .HasColumnName("FechaEntrada").HasColumnType("timestamp(6)");

        builder.Entity<InventoryByBatch>().Property(b => b.Cantidad)
            .HasColumnName("Cantidad");

        builder.Entity<InventoryByBatch>().Property(b => b.Precio)
            .HasColumnName("Precio").HasColumnType("decimal(10,2)");

        builder.Entity<InventoryByBatch>().Property(b => b.Unidad)
            .HasColumnName("Unidad").HasMaxLength(50);

        builder.Entity<InventoryByBatch>().Ignore(b => b.Total); // Propiedad calculada

        builder.Entity<InventoryByBatch>().ToTable("inventory_by_batch");

        // Configuración de Branch (Sucursales)
        builder.Entity<Branch>().HasKey(b => b.Id);
        builder.Entity<Branch>().Property(b => b.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Branch>().Property(b => b.Name).IsRequired().HasMaxLength(200);
        builder.Entity<Branch>().Property(b => b.Type).IsRequired().HasMaxLength(50);
        builder.Entity<Branch>().Property(b => b.Address).IsRequired().HasMaxLength(500);
        builder.Entity<Branch>().Property(b => b.Latitude).IsRequired().HasColumnType("decimal(10,8)");
        builder.Entity<Branch>().Property(b => b.Longitude).IsRequired().HasColumnType("decimal(11,8)");
        builder.Entity<Branch>().Property(b => b.StockTotal).IsRequired().HasDefaultValue(0);
        builder.Entity<Branch>().Property(b => b.CreatedAt).IsRequired().HasColumnType("timestamp(6)");
        builder.Entity<Branch>().Property(b => b.UpdatedAt).HasColumnType("timestamp(6)");
        builder.Entity<Branch>().ToTable("branches");

        
        // Configuración de ProductManagement entities
        
        // Categories
        builder.Entity<Category>().HasKey(c => c.Id);
        builder.Entity<Category>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Category>().Property(c => c.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Category>().ToTable("categories");

        // Units
        builder.Entity<Unit>().HasKey(u => u.Id);
        builder.Entity<Unit>().Property(u => u.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Unit>().Property(u => u.Name).IsRequired().HasMaxLength(100);
        builder.Entity<Unit>().Property(u => u.Abbreviation).IsRequired().HasMaxLength(10);
        builder.Entity<Unit>().ToTable("units_of_measure");

        // Tags
        builder.Entity<Tag>().HasKey(t => t.Id);
        builder.Entity<Tag>().Property(t => t.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Tag>().Property(t => t.Name).IsRequired().HasMaxLength(50);
        builder.Entity<Tag>().ToTable("tags");

        // Products
        builder.Entity<Product>().HasKey(p => p.Id);
        builder.Entity<Product>().Property(p => p.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Product>().Property(p => p.Name).IsRequired().HasMaxLength(200);
        builder.Entity<Product>().Property(p => p.Description).HasColumnType("text");
        builder.Entity<Product>().Property(p => p.PurchasePrice).HasColumnType("decimal(10,2)");
        builder.Entity<Product>().Property(p => p.SalePrice).HasColumnType("decimal(10,2)");
        builder.Entity<Product>().Property(p => p.InternalNotes).HasColumnType("text");

        // Propiedades simples para las claves foráneas
        builder.Entity<Product>().Property(p => p.CategoryId).HasColumnName("CategoryId");
        builder.Entity<Product>().Property(p => p.UnitId).HasColumnName("UnitId");

        // Relaciones de Product
        builder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>()
            .HasOne(p => p.Unit)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.UnitId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>().ToTable("products");

        // ProductTags (tabla intermedia)
        builder.Entity<ProductTag>().HasKey(pt => pt.Id);
        builder.Entity<ProductTag>().Property(pt => pt.Id).IsRequired().ValueGeneratedOnAdd();
        
        // Propiedades simples para las claves foráneas
        builder.Entity<ProductTag>().Property(pt => pt.ProductId).HasColumnName("ProductId");
        builder.Entity<ProductTag>().Property(pt => pt.TagId).HasColumnName("TagId");

        // Relaciones de ProductTag
        builder.Entity<ProductTag>()
            .HasOne(pt => pt.Product)
            .WithMany(p => p.ProductTags)
            .HasForeignKey(pt => pt.ProductId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.Entity<ProductTag>()
            .HasOne(pt => pt.Tag)
            .WithMany(t => t.ProductTags)
            .HasForeignKey(pt => pt.TagId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ProductTag>().ToTable("product_tags");

        // Índice compuesto para evitar duplicados en ProductTag
        builder.Entity<ProductTag>()
            .HasIndex(pt => new { pt.ProductId, pt.TagId })
            .IsUnique();

        // Reports - CategoryReport
        builder.Entity<CategoryReport>().HasKey(r => r.Id);
        builder.Entity<CategoryReport>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<CategoryReport>().Property(r => r.Categoria).IsRequired().HasMaxLength(100);
        builder.Entity<CategoryReport>().Property(r => r.Producto).IsRequired().HasMaxLength(100);
        builder.Entity<CategoryReport>().Property(r => r.FechaConsulta).IsRequired();
        builder.Entity<CategoryReport>().Property(r => r.PrecioUnitario).HasColumnType("decimal(10,2)");
        builder.Entity<CategoryReport>().Property(r => r.Cantidad).IsRequired();
        builder.Entity<CategoryReport>().Ignore(r => r.Total); // Es propiedad calculada
        builder.Entity<CategoryReport>().ToTable("category_reports");

        // Reports - StockAverageReport
        builder.Entity<StockAverageReport>().HasKey(r => r.Id);
        builder.Entity<StockAverageReport>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<StockAverageReport>().Property(r => r.Categoria).IsRequired().HasMaxLength(100);
        builder.Entity<StockAverageReport>().Property(r => r.Producto).IsRequired().HasMaxLength(100);
        builder.Entity<StockAverageReport>().Property(r => r.StockPromedio).HasColumnType("decimal(10,2)");
        builder.Entity<StockAverageReport>().Property(r => r.StockIdeal).IsRequired();
        builder.Entity<StockAverageReport>().Property(r => r.Estado).IsRequired().HasMaxLength(50);
        builder.Entity<StockAverageReport>().Property(r => r.FechaConsulta).IsRequired();
        builder.Entity<StockAverageReport>().ToTable("stock_average_reports");

        

        // Configuración de Combos
        builder.Entity<Combo>().HasKey(c => c.Id);
        builder.Entity<Combo>().Property(c => c.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Combo>().Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Entity<Combo>().ToTable("combos");

        // ComboItems
        builder.Entity<ComboItem>().HasKey(ci => ci.Id);
        builder.Entity<ComboItem>().Property(ci => ci.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<ComboItem>().Property(ci => ci.ComboId).HasColumnName("ComboId");
        builder.Entity<ComboItem>().Property(ci => ci.ProductId).HasColumnName("ProductId");
        builder.Entity<ComboItem>().Property(ci => ci.Quantity).IsRequired();

        // Relaciones de ComboItem
        builder.Entity<ComboItem>()
            .HasOne(ci => ci.Combo)
            .WithMany(c => c.ComboItems)
            .HasForeignKey(ci => ci.ComboId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ComboItem>()
            .HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ComboItem>().ToTable("combo_items");
    }
}