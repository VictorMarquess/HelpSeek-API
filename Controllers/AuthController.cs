using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using HelpSeek.API.Models;

namespace HelpSeek.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        public AuthController(IConfiguration config) => _config = config;
        private SqlConnection Conn() => new SqlConnection(_config.GetConnectionString("DefaultConnection"));

        // 🔹 Calcula o hash SHA256 igual ao do script SQL
        private static string CalcularHash(string email, string senha)
        {
            using var sha = SHA256.Create();
            string entrada = $"{email.ToLower()}:{senha}";
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(entrada));
            var sb = new StringBuilder();
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var hash = CalcularHash(dto.Email, dto.Senha);

            using var conn = Conn();
            await conn.OpenAsync();

            var sql = @"SELECT Id, Nome, Papel FROM Usuarios WHERE LOWER(Email)=@e AND SenhaHash=@s";
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@e", dto.Email.ToLower());
            cmd.Parameters.AddWithValue("@s", hash);

            using var r = await cmd.ExecuteReaderAsync();
            if (await r.ReadAsync())
            {
                return Ok(new
                {
                    Id = r.GetInt32(0),
                    Nome = r.GetString(1),
                    Papel = r.GetString(2)
                });
            }

            return Unauthorized();
        }
    }
}
