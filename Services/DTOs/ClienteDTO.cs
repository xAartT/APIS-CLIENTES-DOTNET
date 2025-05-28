using System;

namespace ApiClientes.Services.DTOs
{
    public class CriarClienteDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public DateTime? Nascimento { get; set; }

        public string Telefone { get; set; }

        public string Documento { get; set; }

    }

    public class ClienteDTO
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public DateTime? Nascimento { get; set; }

        public string Telefone { get; set; }

        public string Documento { get; set; }


        public DateTime Criadoem { get; set; }

        public DateTime Alteradoem { get; set; }
    }
}
