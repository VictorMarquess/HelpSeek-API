using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using HelpSeek.API.Models;

namespace HelpSeek.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AvaliacoesController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AvaliacoesController(IConfiguration config) => _config = config;
        private SqlConnection Conn() => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        [HttpPost]
        public async Task<IActionResult> Post(AvaliacaoDto dto)
        {
            using var conn = Conn();
            await conn.OpenAsync();
            var cmd = new SqlCommand(@"INSERT INTO Feedback (ChamadoId, UsuarioId, Avaliacao, Comentario, CriadoEm)
                                       VALUES (@c,@u,@a,@cm,GETDATE())", conn);
            cmd.Parameters.AddWithValue("@c", dto.ChamadoId);
            cmd.Parameters.AddWithValue("@u", dto.UsuarioId);
            cmd.Parameters.AddWithValue("@a", dto.Avaliacao);
            cmd.Parameters.AddWithValue("@cm", dto.Comentario ?? "");
            await cmd.ExecuteNonQueryAsync();
            return Ok(new { success = true });
        }
    }
}
