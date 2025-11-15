namespace HelpSeek.API.Models
{
    public class SimpleChamado
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public string? Status { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
