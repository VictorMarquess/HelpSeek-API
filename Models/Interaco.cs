using System;
using System.Collections.Generic;

namespace HelpSeek.API.Models;

public partial class Interaco
{
    public int Id { get; set; }

    public int ChamadoId { get; set; }

    public int UsuarioId { get; set; }

    public string Origem { get; set; } = null!;

    public string Mensagem { get; set; } = null!;

    public DateTime CriadoEm { get; set; }

    public virtual Chamado Chamado { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
