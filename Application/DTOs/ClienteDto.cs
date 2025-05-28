using System;

namespace App.Application.DTOs;

public class ClienteDto
{
    public string Nome { get; set; } = null!;
    public DateTime? Nascimento { get; set; }
    public string Telefone { get; set; }
    public string Documento { get; set; } = null!;
}
