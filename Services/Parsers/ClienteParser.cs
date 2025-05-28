using ApiClientes.Database.Models;
using ApiClientes.Services.DTOs;

namespace ApiClientes.Services.Parsers
{
    public class ClienteParser
    {
        public static TbCliente ToTbCliente(
                     CriarClienteDTO dto)
        {
            TbCliente novoCliente = new();
            novoCliente.Nome = dto.Nome;
            novoCliente.Telefone = dto.Telefone;
            novoCliente.Nascimento = dto.Nascimento;
            novoCliente.Documento = dto.Documento;
            novoCliente.Criadoem = System.DateTime.Now.ToUniversalTime();
            novoCliente.Alteradoem = novoCliente.Criadoem;

            return novoCliente;
        }

        public static ClienteDTO ToClienteDTO(TbCliente cliente)
        {
            ClienteDTO Response = new();
            Response.Nome = cliente.Nome;
            Response.Criadoem = cliente.Criadoem;
            Response.Alteradoem = cliente.Alteradoem;
            Response.Telefone = cliente.Telefone;
            Response.Documento = cliente.Documento;
            Response.Nascimento = cliente.Nascimento;

            return Response;

        }


    }
}
