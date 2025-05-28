using App.Domain.Entities;
using App.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace App.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Endereco> Enderecos => Set<Endereco>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");

        // Helper para convertir PascalCase a snake_case
        static string ToSnakeCase(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(
                input,
                @"([a-z0-9])([A-Z])",
                "$1_$2",
                RegexOptions.Compiled
            ).ToLower();
        }

        // Método para aplicar HasColumnName con snake_case para todas las propiedades
        static void MapEntityProperties<TEntity>(EntityTypeBuilder<TEntity> entity) where TEntity : class
        {
            var properties = typeof(TEntity).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in properties)
            {
                // Ignorar propiedades que EF trata como navegación:
                // 1) Si es colección (IEnumerable excepto string)
                // 2) Si es clase compleja no string (entidad)
                if (typeof(IEnumerable).IsAssignableFrom(prop.PropertyType) && prop.PropertyType != typeof(string))
                    continue;

                if (prop.PropertyType.IsClass && prop.PropertyType != typeof(string))
                    continue;

                var columnName = ToSnakeCase(prop.Name);
                entity.Property(prop.Name).HasColumnName(columnName);
            }
        }

        // Cliente entity config
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("clientes");
            entity.HasKey(e => e.Id);

            MapEntityProperties(entity);

            entity.Property(e => e.Nome).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Documento).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Telefone).HasMaxLength(20).IsRequired(false);
            entity.Property(e => e.CriadoEm).IsRequired();
            entity.Property(e => e.AlteradoEm).IsRequired();
        });

        // Endereco entity config
        modelBuilder.Entity<Endereco>(static entity =>
        {
            entity.ToTable("enderecos");
            entity.HasKey(e => e.Id);

            MapEntityProperties(entity);

            entity.Property(e => e.Cep).IsRequired();
            entity.Property(e => e.Logradouro).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Numero).HasMaxLength(20).IsRequired();
            entity.Property(e => e.Complemento).HasMaxLength(255).IsRequired(false);
            entity.Property(e => e.Bairro).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Cidade).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Uf).HasMaxLength(2).IsRequired();
            entity.Property(e => e.Status).IsRequired().HasDefaultValue(StatusEndereco.Ativo);
        });
    }
    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            var props = entry.Properties.Where(p => p.Metadata.ClrType == typeof(DateTime));
            foreach (var prop in props)
            {
                if (prop.CurrentValue is DateTime dt)
                {
                    if (dt.Kind == DateTimeKind.Unspecified)
                    {
                        prop.CurrentValue = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                    }
                    else if (dt.Kind == DateTimeKind.Local)
                    {
                        prop.CurrentValue = dt.ToUniversalTime();
                    }
                }
            }
        }
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
        {
            var props = entry.Properties.Where(p => p.Metadata.ClrType == typeof(DateTime));
            foreach (var prop in props)
            {
                if (prop.CurrentValue is DateTime dt)
                {
                    if (dt.Kind == DateTimeKind.Unspecified)
                    {
                        prop.CurrentValue = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                    }
                    else if (dt.Kind == DateTimeKind.Local)
                    {
                        prop.CurrentValue = dt.ToUniversalTime();
                    }
                }
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
