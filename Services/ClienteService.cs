using System.Net.Http;
using ApiClientes.Database.Models;
using ApiClientes.Services.DTOs;
using ApiClientes.Services.Parsers;
using ApiClientes.Services.Validations;
using Microsoft.AspNetCore.Http.HttpResults;

namespace ApiClientes.Services
{
    public class ClienteService
    {

        private readonly ClientesContext _dbcontext;


        public ClienteService(ClientesContext dbcontext)
        { 
            _dbcontext = dbcontext;

        }

        public ClienteDTO Criar(CriarClienteDTO dto)
        {
            ClienteValidation.ValidouCriarCliente(dto);

            TbCliente novoCliente =
                ClienteParser.ToTbCliente(dto);
         
            _dbcontext.TbClientes.Add(novoCliente);
            _dbcontext.SaveChanges();

            return ClienteParser.ToClienteDTO(novoCliente);

        }


    }
}
