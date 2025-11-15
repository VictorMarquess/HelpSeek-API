using System;
using System.Collections.Generic;

namespace HelpSeek.API.Models;

public partial class Prioridade
{
    public int PrioridadeId { get; set; }

    public string Nivel { get; set; } = null!;

    public virtual ICollection<Chamado> Chamados { get; set; } = new List<Chamado>();
}
