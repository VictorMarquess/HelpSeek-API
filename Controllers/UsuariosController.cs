using Microsoft.AspNetCore.Mvc;
using HelpSeek.API.Models;
using System.Security.Cryptography;
using System.Text;

namespace HelpSeek.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly HelpSeekContext _context;

        public UsuariosController(HelpSeekContext context)
        {
            _context = context;
        }

        // 🔹 Criptografa a senha (mesmo padrão do login)
        private static string GerarHash(string email, string senha)
        {
            using var sha = SHA256.Create();
            string entrada = $"{email.ToLower()}:{senha}";
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(entrada));
            return BitConverter.ToString(bytes).Replace("-", "").ToLower();
        }

        // ✅ POST: /api/usuarios
        [HttpPost]
        public IActionResult CriarUsuario([FromBody] Usuario dto)
        {
            if (dto == null)
                return BadRequest("Dados inválidos.");

            if (string.IsNullOrWhiteSpace(dto.Nome) ||
                string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.Papel) ||
                string.IsNullOrWhiteSpace(dto.SenhaHash))
            {
                return BadRequest("Campos obrigatórios não informados.");
            }

            // Verifica se o e-mail já existe
            if (_context.Usuarios.Any(u => u.Email.ToLower() == dto.Email.ToLower()))
                return BadRequest("Já existe um usuário com este e-mail.");

            // Gera hash da senha
            string senhaHash = GerarHash(dto.Email, dto.SenhaHash);

            var usuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Papel = dto.Papel,
                SenhaHash = senhaHash,
                Departamento = dto.Departamento
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            return Ok(new
            {
                mensagem = "Usuário criado com sucesso!",
                usuario.Id,
                usuario.Nome,
                usuario.Email,
                usuario.Papel
            });
        }
    }
}
