using System;
using System.Collections.Generic;

namespace HelpSeek.API.Models;

public partial class Feedback
{
    public int Id { get; set; }

    public int ChamadoId { get; set; }

    public int UsuarioId { get; set; }

    public int Nota { get; set; }

    public string? Comentario { get; set; }

    public DateTime CriadoEm { get; set; }

    public virtual Chamado Chamado { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
