using System.Collections.Generic;
using System.Threading.Tasks;
using App.Application.DTOs;
using App.Domain.Entities;

namespace App.Application.Interfaces;

public interface IClienteService
{
    Task<List<Cliente>> GetAll();
    Task<Cliente> GetById(int id);
    Task<Cliente> Create(ClienteDto dto);
    Task<Cliente> Update(int id, ClienteDto dto);
    Task<bool> Delete(int id);
}
