using Lot.IAM.Domain.Model.Aggregates;
using Lot.ProductManagement.Domain.Model.Aggregates;
using Lot.Reports.Domain.Model.Aggregates;
using Lot.Sales.Domain.Model.Aggregates;
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
        builder.Entity<User>().Property(u => u.Email).IsRequired().HasMaxLength(255);

        // Agregar índice único para email (evita duplicados)
        builder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique()
            .HasDatabaseName("IX_Users_Email_Unique");

        // Configuración de Inventory
        //by-product
        builder.Entity<InventoryByProduct>().HasKey(p => p.Id);

        builder.Entity<InventoryByProduct>().Property(p => p.Id)
            .IsRequired().ValueGeneratedOnAdd();

        // FK y campos principales
        builder.Entity<InventoryByProduct>().Property(p => p.ProductoId)
            .HasColumnName("ProductoId");

        builder.Entity<InventoryByProduct>().Property(p => p.FechaEntrada)
            .HasColumnName("FechaEntrada").HasColumnType("timestamp(6)");

        builder.Entity<InventoryByProduct>().Property(p => p.Cantidad)
            .HasColumnName("Cantidad");

        builder.Entity<InventoryByProduct>().Property(p => p.Precio)
            .HasColumnName("Precio").HasColumnType("decimal(10,2)");

        builder.Entity<InventoryByProduct>().Property(p => p.StockMinimo)
            .HasColumnName("StockMinimo");

        builder.Entity<InventoryByProduct>().ToTable("inventory_by_product");

        // NUEVO: Relación con Products (si existe FK)
        builder.Entity<InventoryByProduct>()
            .HasOne(p => p.Product)
            .WithMany()
            .HasForeignKey(p => p.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        // NUEVO: Índice para mejor performance en FK (compatible con múltiples BD)
        builder.Entity<InventoryByProduct>()
            .HasIndex(p => p.ProductoId)
            .HasDatabaseName("IX_InventoryByProduct_ProductoId");

        //by-bach
        builder.Entity<InventoryByBatch>().HasKey(b => b.Id);

        builder.Entity<InventoryByBatch>().Property(b => b.Id)
            .IsRequired().ValueGeneratedOnAdd();

        // NUEVO: Foreign Key a Products
        builder.Entity<InventoryByBatch>().Property(b => b.ProductoId)
            .HasColumnName("ProductoId");

        builder.Entity<InventoryByBatch>().Property(b => b.Proveedor)
            .HasColumnName("Proveedor").IsRequired().HasMaxLength(100);

        // NUEVO: Foreign Key a Units
        builder.Entity<InventoryByBatch>().Property(b => b.UnidadId)
            .HasColumnName("UnidadId");

        
        builder.Entity<InventoryByBatch>().Property(b => b.FechaEntrada)
            .HasColumnName("FechaEntrada").HasColumnType("timestamp(6)");

        builder.Entity<InventoryByBatch>().Property(b => b.Cantidad)
            .HasColumnName("Cantidad");

        builder.Entity<InventoryByBatch>().Property(b => b.Precio)
            .HasColumnName("Precio").HasColumnType("decimal(10,2)");

        
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
        // NUEVO: Relación con Products (si existe FK)
        builder.Entity<InventoryByBatch>()
            .HasOne(b => b.Product)
            .WithMany()
            .HasForeignKey(b => b.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        // NUEVO: Relación con Units
        builder.Entity<InventoryByBatch>()
            .HasOne(b => b.Unit)
            .WithMany()
            .HasForeignKey(b => b.UnidadId)
            .OnDelete(DeleteBehavior.Restrict);

        // NUEVO: Índices para mejor performance en FKs (compatible con múltiples BD)
        builder.Entity<InventoryByBatch>()
            .HasIndex(b => b.ProductoId)
            .HasDatabaseName("IX_InventoryByBatch_ProductoId");

        builder.Entity<InventoryByBatch>()
            .HasIndex(b => b.UnidadId)
            .HasDatabaseName("IX_InventoryByBatch_UnidadId");


        
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
        builder.Entity<CategoryReport>().Property(r => r.CategoryId).IsRequired();
        builder.Entity<CategoryReport>().Property(r => r.CategoriaNombre).IsRequired().HasMaxLength(100);
        builder.Entity<CategoryReport>().Property(r => r.FechaConsulta).IsRequired();
        builder.Entity<CategoryReport>().Property(r => r.TotalProductos).IsRequired();
        builder.Entity<CategoryReport>().Property(r => r.StockTotal).IsRequired();
        builder.Entity<CategoryReport>().Property(r => r.ValorTotalInventario).HasColumnType("decimal(12,2)");
        builder.Entity<CategoryReport>().Property(r => r.ProductosBajoStock).IsRequired();
        builder.Entity<CategoryReport>().ToTable("category_reports");

        // Reports - StockAverageReport
        builder.Entity<StockAverageReport>().HasKey(r => r.Id);
        builder.Entity<StockAverageReport>().Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<StockAverageReport>().Property(r => r.CategoryId).IsRequired();
        builder.Entity<StockAverageReport>().Property(r => r.CategoriaNombre).IsRequired().HasMaxLength(100);
        builder.Entity<StockAverageReport>().Property(r => r.FechaConsulta).IsRequired();
        builder.Entity<StockAverageReport>().Property(r => r.StockPromedioReal).HasColumnType("decimal(10,2)");
        builder.Entity<StockAverageReport>().Property(r => r.StockMinimoPromedio).IsRequired();
        builder.Entity<StockAverageReport>().Property(r => r.TotalProductos).IsRequired();
        builder.Entity<StockAverageReport>().Property(r => r.ProductosBajoStock).IsRequired();
        builder.Entity<StockAverageReport>().Property(r => r.PorcentajeBajoStock).HasColumnType("decimal(5,2)");
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

        // Configuración de Sales Module

        // Sales
        builder.Entity<Sale>().HasKey(s => s.Id);
        builder.Entity<Sale>().Property(s => s.Id).IsRequired().ValueGeneratedOnAdd();
        builder.Entity<Sale>().Property(s => s.ProductId).IsRequired();
        builder.Entity<Sale>().Property(s => s.ProductName).IsRequired().HasMaxLength(200);
        builder.Entity<Sale>().Property(s => s.CategoryName).HasMaxLength(100);
        builder.Entity<Sale>().Property(s => s.SaleDate).IsRequired();
        builder.Entity<Sale>().OwnsOne(s => s.Quantity, q =>
        {
            q.Property(p => p.Value).HasColumnName("Quantity").IsRequired();
        });

        builder.Entity<Sale>().OwnsOne(s => s.UnitPrice, up =>
        {
            up.Property(p => p.Value).HasColumnName("UnitPrice").HasColumnType("decimal(10,2)").IsRequired();
        });
        builder.Entity<Sale>().Property(s => s.CustomerName).IsRequired().HasMaxLength(200);
        builder.Entity<Sale>().Property(s => s.Notes).HasMaxLength(500);

        builder.Entity<Sale>().Ignore(s => s.TotalAmount); // Propiedad calculada

        // ⭐ NUEVOS campos para combos
        builder.Entity<Sale>().Property(s => s.ComboId);
        builder.Entity<Sale>().Property(s => s.ComboName).HasColumnType("varchar(200)");

        builder.Entity<Sale>().ToTable("sales");

        // Índices para mejor performance en ventas
        builder.Entity<Sale>()
            .HasIndex(s => s.ProductId)
            .HasDatabaseName("IX_Sales_ProductId");

        builder.Entity<Sale>()
            .HasIndex(s => s.SaleDate)
            .HasDatabaseName("IX_Sales_SaleDate");

        builder.Entity<Sale>()
            .HasIndex(s => s.CustomerName)
            .HasDatabaseName("IX_Sales_CustomerName");

        // ⨳ NUEVO: Índice para combos
        builder.Entity<Sale>()
            .HasIndex(s => s.ComboId)
            .HasDatabaseName("IX_Sales_ComboId");
    }
}