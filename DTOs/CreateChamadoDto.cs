namespace HelpSeek.API.DTOs
{
    public class CreateChamadoDto
    {
        public required string Titulo { get; set; }
        public required string Descricao { get; set; }
        public required string Status { get; set; }      // "Aberto", "Em Andamento", etc.
        public required string Prioridade { get; set; }  // "Alta", "Média", "Baixa"
        public required string Categoria { get; set; }    // "Aplicação", "Rede", etc.
        public int UsuarioId { get; set; }
    }
}
