using System;
using System.Collections.Generic;

namespace ApiClientes.Database.Models;

public partial class TbCliente
{
    public int Id { get; set; }

    public string Nome { get; set; }

    public DateTime? Nascimento { get; set; }

    public string Telefone { get; set; }

    public string Documento { get; set; }

    public DateTime Criadoem { get; set; }

    public DateTime Alteradoem { get; set; }

    public virtual ICollection<TbEndereco> TbEnderecos { get; set; } = new List<TbEndereco>();
}
