using System.Linq;
using ApiClientes.Services.DTOs;
using ApiClientes.Services.Exceptions;

namespace ApiClientes.Services.Validations
{
    public class ClienteValidation
    {
       public static bool ValidouCriarCliente(
           CriarClienteDTO criarClienteDTO)
        {
            if (string.IsNullOrEmpty(criarClienteDTO.Nome))
                throw new BadRequestException("Nome é obrigatório");

            if (string.IsNullOrEmpty(criarClienteDTO.Documento))
                throw new BadRequestException("Documento é obrigatório");

            return true;
        }

    }
}
