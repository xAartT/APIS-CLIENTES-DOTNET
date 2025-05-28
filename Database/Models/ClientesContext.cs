using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiClientes.Database.Models;

public partial class ClientesContext : DbContext
{
    public ClientesContext()
    {
    }

    public ClientesContext(DbContextOptions<ClientesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TbCliente> TbClientes { get; set; }

    public virtual DbSet<TbEndereco> TbEnderecos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost;Database=apiClientes;Username=postgres;Password=postgres");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TbCliente>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tb_clientes");

            entity.ToTable("tb_clientes");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Alteradoem)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("alteradoem");
            entity.Property(e => e.Criadoem)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("criadoem");
            entity.Property(e => e.Documento)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("documento");
            entity.Property(e => e.Nascimento)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("nascimento");
            entity.Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("nome");
            entity.Property(e => e.Telefone)
                .HasMaxLength(20)
                .HasColumnName("telefone");
        });

        modelBuilder.Entity<TbEndereco>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_tb_enderecos");

            entity.ToTable("tb_enderecos");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Bairro)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("bairro");
            entity.Property(e => e.Cep).HasColumnName("cep");
            entity.Property(e => e.Cidade)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("cidade");
            entity.Property(e => e.Clienteid).HasColumnName("clienteid");
            entity.Property(e => e.Complemento)
                .HasMaxLength(255)
                .HasColumnName("complemento");
            entity.Property(e => e.Logradouro)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("logradouro");
            entity.Property(e => e.Numero)
                .IsRequired()
                .HasMaxLength(20)
                .HasColumnName("numero");
            entity.Property(e => e.Status)
                .HasDefaultValue(1)
                .HasComment("0 - inativo\\n1 - ativo")
                .HasColumnName("status");
            entity.Property(e => e.Uf)
                .IsRequired()
                .HasMaxLength(2)
                .HasColumnName("uf");

            entity.HasOne(d => d.Cliente).WithMany(p => p.TbEnderecos)
                .HasForeignKey(d => d.Clienteid)
                .HasConstraintName("fk_tb_enderecos_tb_clientes");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
