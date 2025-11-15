using System;
using System.Collections.Generic;

namespace HelpSeek.API.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Papel { get; set; } = null!;

    public string SenhaHash { get; set; } = null!;

    public string? Departamento { get; set; }

    public virtual ICollection<Chamado> ChamadoResponsavels { get; set; } = new List<Chamado>();

    public virtual ICollection<Chamado> ChamadoUsuarios { get; set; } = new List<Chamado>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<Interaco> Interacos { get; set; } = new List<Interaco>();
}
