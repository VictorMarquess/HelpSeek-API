using System;
using System.Collections.Generic;

namespace HelpSeek.API.Models;

public partial class Chamado
{
    public int Id { get; set; }

    public string Titulo { get; set; } = null!;

    public string? Descricao { get; set; }

    public int UsuarioId { get; set; }

    public int StatusId { get; set; }

    public int CategoriaId { get; set; }

    public int PrioridadeId { get; set; }

    public int? SistemaOrigemId { get; set; }

    public string? EmailUsuario { get; set; }

    public int? ResponsavelId { get; set; }

    public DateTime CriadoEm { get; set; }

    public DateTime? AtualizadoEm { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Interaco> Interacos { get; set; } = new List<Interaco>();

    public virtual Prioridade Prioridade { get; set; } = null!;

    public virtual Usuario? Responsavel { get; set; }

    public virtual Sistema? SistemaOrigem { get; set; }

    public virtual Status Status { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
