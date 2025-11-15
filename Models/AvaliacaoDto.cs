namespace HelpSeek.API.Models
{
    public class AvaliacaoDto
    {
        public int ChamadoId { get; set; }
        public int UsuarioId { get; set; }
        public int Avaliacao { get; set; }
        public string? Comentario { get; set; }
    }
}
