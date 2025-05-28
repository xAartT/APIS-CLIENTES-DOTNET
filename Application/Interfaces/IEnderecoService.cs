using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.DTOs;
using App.Domain.Entities;

namespace App.Application.Interfaces;

public interface IEnderecoService
{
    Task<List<Endereco>> GetByCliente(int clienteId);
    Task<Endereco> GetById(int id);
    Task<Endereco> Create(EnderecoDto dto);
    Task<Endereco> Update(int id, EnderecoDto dto);
    Task<bool> Delete(int id);
    Task<bool> Disable(int id);
    Task<bool> Enable(int id);
}
