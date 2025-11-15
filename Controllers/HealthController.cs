using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace HelpSeek.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public HealthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("db")]
        public async Task<IActionResult> CheckDatabase()
        {
            var connStr = _config.GetConnectionString("DefaultConnection");
            try
            {
                using var conn = new SqlConnection(connStr);
                await conn.OpenAsync();

                // Conta as tabelas e retorna algumas infos do servidor
                using var cmd = new SqlCommand("""
                    SELECT TOP 1
                      @@SERVERNAME AS ServerName,
                      DB_NAME() AS DatabaseName;
                """, conn);
                using var reader = await cmd.ExecuteReaderAsync();
                string server = "", db = "";
                if (await reader.ReadAsync())
                {
                    server = reader["ServerName"]?.ToString() ?? "";
                    db = reader["DatabaseName"]?.ToString() ?? "";
                }
                reader.Close();

                using var cmdCount = new SqlCommand("SELECT COUNT(*) FROM sys.tables", conn);
                int tables = (int)await cmdCount.ExecuteScalarAsync();

                return Ok(new { ok = true, server, database = db, tables });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { ok = false, error = ex.Message });
            }
        }
    }
}
