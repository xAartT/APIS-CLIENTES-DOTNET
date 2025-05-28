using System;
using App.Domain.Enums;

namespace App.Domain.Entities;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public DateTime? Nascimento { get; set; }
    public string Telefone { get; set; }
    public string Documento { get; set; } = null!;
    public DateTime CriadoEm { get; set; }
    public DateTime AlteradoEm { get; set; }
}
