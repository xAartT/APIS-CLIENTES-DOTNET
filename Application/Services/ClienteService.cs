using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.DTOs;
using App.Application.Interfaces;
using App.Domain.Entities;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using App.Domain.Enums;
using App.Domain.Error;

namespace App.Application.Services;

public class ClienteService(ApplicationDbContext context) : IClienteService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Cliente>> GetAll()
    {
        return await _context.Clientes
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Cliente> GetById(int id)
    {
        var cliente = await _context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new AppException("Cliente não encontrado", "Not Found", 404);

        return cliente;
    }

    public async Task<Cliente> Create(ClienteDto dto)
    {
        Validate(dto, isUpdate: false);

        var now = DateTime.UtcNow;

        var cliente = new Cliente
        {
            Nome = dto.Nome!,
            Nascimento = EnsureUtc(dto.Nascimento),
            Telefone = dto.Telefone!,
            Documento = dto.Documento!,
            CriadoEm = now,
            AlteradoEm = now
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return cliente;
    }

    public async Task<Cliente> Update(int id, ClienteDto dto)
    {
        var cliente = await _context.Clientes.FindAsync(id)
            ?? throw new AppException("Cliente não encontrado", "Not Found", 404);

        var campos = 0;

        if (!string.IsNullOrWhiteSpace(dto.Nome))
        {
            cliente.Nome = dto.Nome!;
            campos++;
        }

        if (dto.Nascimento.HasValue)
        {
            cliente.Nascimento = EnsureUtc(dto.Nascimento.Value);
            campos++;
        }

        if (!string.IsNullOrWhiteSpace(dto.Telefone))
        {
            cliente.Telefone = dto.Telefone!;
            campos++;
        }

        if (!string.IsNullOrWhiteSpace(dto.Documento))
        {
            cliente.Documento = dto.Documento!;
            campos++;
        }

        if (campos == 0)
            throw new AppException("Nenhum dado informado para atualizar", "Bad Request", 400);

        cliente.AlteradoEm = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return cliente;
    }

    public async Task<bool> Delete(int id)
    {
        var cliente = await _context.Clientes.FindAsync(id)
            ?? throw new AppException("Cliente não encontrado", "Not Found", 404);

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return true;
    }

    private static DateTime? EnsureUtc(DateTime? input)
    {
        if (!input.HasValue) return null;

        var value = input.Value;

        return value.Kind == DateTimeKind.Utc
            ? value
            : DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

    private static void Validate(ClienteDto dto, bool isUpdate)
    {
        if (isUpdate) return;

        if (
            string.IsNullOrWhiteSpace(dto.Nome)
            || string.IsNullOrWhiteSpace(dto.Telefone)
            || string.IsNullOrWhiteSpace(dto.Documento)
            || !dto.Nascimento.HasValue
        )
        {
            throw new AppException("Dados incompletos", "Bad Request", 400);
        }
    }
}
