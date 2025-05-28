using App.Domain.Enums;

namespace App.Domain.Entities;

public class Endereco
{
    public int Id { get; set; }
    public int Cep { get; set; }
    public string Logradouro { get; set; } = null!;
    public string Numero { get; set; } = null!;
    public string Complemento { get; set; }
    public string Bairro { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string Uf { get; set; } = null!;
    public int ClienteId { get; set; }
    public StatusEndereco? Status { get; set; }

    public Cliente Cliente { get; set; } = null!;
}
