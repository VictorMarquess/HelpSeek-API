using System;
using System.Collections.Generic;

namespace HelpSeek.API.Models;

public partial class Status
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Chamado> Chamados { get; set; } = new List<Chamado>();
}
