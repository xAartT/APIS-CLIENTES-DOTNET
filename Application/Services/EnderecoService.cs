using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Application.DTOs;
using App.Application.Interfaces;
using App.Domain.Entities;
using App.Domain.Enums;
using App.Domain.Error;
using App.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace App.Application.Services;

public class EnderecoService(ApplicationDbContext context) : IEnderecoService
{
    private readonly ApplicationDbContext _context = context;

    public async Task<List<Endereco>> GetByCliente(int clienteId)
    {
        var clienteExists = await _context.Clientes
            .AnyAsync(c => c.Id == clienteId);

        if (!clienteExists)
            throw new AppException("Cliente não encontrado", "Not Found", 404);

        return await _context.Enderecos
            .Where(e => e.ClienteId == clienteId && e.Status == StatusEndereco.Ativo)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Endereco> GetById(int id)
    {
        var endereco = await _context.Enderecos
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id)
            ?? throw new AppException("Endereço não encontrado", "Not Found", 404);

        return endereco;
    }

    public async Task<Endereco> Create(EnderecoDto dto)
    {
        Validate(dto, isUpdate: false);

        var clienteExists = await _context.Clientes
            .AnyAsync(c => c.Id == dto.ClienteId);

        if (!clienteExists)
            throw new AppException("Cliente não encontrado", "Not Found", 404);

        var endereco = new Endereco
        {
            Cep = dto.Cep!.Value,
            Logradouro = dto.Logradouro!,
            Numero = dto.Numero!,
            Complemento = dto.Complemento,
            Bairro = dto.Bairro!,
            Cidade = dto.Cidade!,
            Uf = dto.Uf!,
            ClienteId = dto.ClienteId!.Value,
            Status = StatusEndereco.Ativo
        };

        _context.Enderecos.Add(endereco);
        await _context.SaveChangesAsync();
        return endereco;
    }

    public async Task<Endereco> Update(int id, EnderecoDto dto)
    {
        var campos = 0;

        var endereco = await _context.Enderecos.FindAsync(id)
            ?? throw new AppException("Endereço não encontrado", "Not Found", 404);

        if (!string.IsNullOrWhiteSpace(dto.Logradouro))
        {
            campos++;
            endereco.Logradouro = dto.Logradouro!;
        }

        if (!string.IsNullOrWhiteSpace(dto.Numero))
        {
            campos++;
            endereco.Numero = dto.Numero!;
        }

        if (!string.IsNullOrWhiteSpace(dto.Complemento))
        {
            campos++;
            endereco.Complemento = dto.Complemento!;
        }

        if (!string.IsNullOrWhiteSpace(dto.Bairro))
        {
            campos++;
            endereco.Bairro = dto.Bairro!;
        }

        if (!string.IsNullOrWhiteSpace(dto.Cidade))
        {
            campos++;
            endereco.Cidade = dto.Cidade!;
        }

        if (!string.IsNullOrWhiteSpace(dto.Uf))
        {
            campos++;
            endereco.Uf = dto.Uf!;
        }

        if (dto.Cep.HasValue)
        {
            campos++;
            endereco.Cep = dto.Cep.Value;
        }

        if (dto.ClienteId.HasValue)
        {
            var clienteExists = await _context.Clientes
                .AnyAsync(c => c.Id == dto.ClienteId.Value);

            if (!clienteExists)
                throw new AppException("Cliente não encontrado", "Not Found", 404);

            campos++;
            endereco.ClienteId = dto.ClienteId.Value;
        }

        if (campos == 0)
            throw new AppException("Nenhum dado informado para atualizar", "Bad Request", 400);

        await _context.SaveChangesAsync();
        return endereco;
    }

    public async Task<bool> Disable(int id)
    {
        var endereco = await _context.Enderecos.FindAsync(id)
            ?? throw new AppException("Endereço não encontrado", "Not Found", 404);

        if (endereco.Status == StatusEndereco.Inativo)
            throw new AppException("Endereço já está inativo", "Conflict", 409);

        endereco.Status = StatusEndereco.Inativo;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Enable(int id)
    {
        var endereco = await _context.Enderecos.FindAsync(id)
            ?? throw new AppException("Endereço não encontrado", "Not Found", 404);

        if (endereco.Status == StatusEndereco.Ativo)
            throw new AppException("Endereço já está ativo", "Conflict", 409);

        endereco.Status = StatusEndereco.Ativo;
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var endereco = await _context.Enderecos.FindAsync(id)
            ?? throw new AppException("Endereço não encontrado", "Not Found", 404);

        _context.Enderecos.Remove(endereco);
        await _context.SaveChangesAsync();

        return true;
    }

    private static void Validate(EnderecoDto dto, bool isUpdate)
    {
        if (isUpdate) return;

        if (
            string.IsNullOrWhiteSpace(dto.Logradouro)
            || string.IsNullOrWhiteSpace(dto.Numero)
            || string.IsNullOrWhiteSpace(dto.Bairro)
            || string.IsNullOrWhiteSpace(dto.Cidade)
            || string.IsNullOrWhiteSpace(dto.Uf)
            || !dto.Cep.HasValue
            || !dto.ClienteId.HasValue
        )
        {
            throw new AppException("Dados incompletos", "Bad Request", 400);
        }
    }
}
