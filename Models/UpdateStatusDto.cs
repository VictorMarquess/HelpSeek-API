namespace HelpSeek.API.Models
{
    public class UpdateStatusDto
    {
        public string Status { get; set; } = "Em andamento";
        public int? TecnicoId { get; set; }
    }
}
