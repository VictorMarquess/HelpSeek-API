using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HelpSeek.API.Models;
using HelpSeek.API.DTOs;
using System.Text.Json;

namespace HelpSeek.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChamadosController : ControllerBase
    {
        private readonly HelpSeekContext _context;

        public ChamadosController(HelpSeekContext context)
        {
            _context = context;
        }

        // ✅ Cria um novo chamado
        [HttpPost]
        public IActionResult Create([FromBody] CreateChamadoDto dto)
        {
            if (dto is null)
                return BadRequest("Requisição inválida.");

            // resolve IDs pelos nomes
            var categoriaId = _context.Set<Categoria>()
                .Where(c => c.Nome == dto.Categoria)
                .Select(c => c.Id)
                .FirstOrDefault();

            var prioridadeId = _context.Set<Prioridade>()
                .Where(p => p.Nivel == dto.Prioridade)
                .Select(p => p.PrioridadeId)
                .FirstOrDefault();

            var statusId = _context.Set<Status>()
                .Where(s => s.Nome == dto.Status)
                .Select(s => s.Id)
                .FirstOrDefault();

            if (categoriaId == 0 || prioridadeId == 0 || statusId == 0)
                return BadRequest("Categoria, prioridade ou status inválidos.");

            var chamado = new Chamado
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                UsuarioId = dto.UsuarioId,
                CategoriaId = categoriaId,
                PrioridadeId = prioridadeId,
                StatusId = statusId,
                CriadoEm = DateTime.Now
            };

            _context.Chamados.Add(chamado);
            _context.SaveChanges();

            return Ok(chamado);
        }

        // ✅ Retorna todos os chamados existentes com todos os campos necessários
        [HttpGet]
        public IActionResult GetAll()
        {
            var chamados = _context.Chamados
                .OrderByDescending(c => c.CriadoEm)
                .Select(c => new
                {
                    c.Id,
                    c.Titulo,
                    c.Descricao,
                    c.CriadoEm,
                    c.UsuarioId,
                    c.ResponsavelId,
                    Categoria = _context.Categorias.FirstOrDefault(cat => cat.Id == c.CategoriaId).Nome,
                    Prioridade = _context.Prioridades.FirstOrDefault(p => p.PrioridadeId == c.PrioridadeId).Nivel,
                    Status = _context.Statuses.FirstOrDefault(s => s.Id == c.StatusId).Nome
                })
                .ToList();

            return Ok(chamados);
        }

        // ✅ Atualiza status, técnico e/ou feedback do chamado
        [HttpPut("{id}")]
        public IActionResult AtualizarChamado(int id, [FromBody] JsonElement dto)
        {
            var chamado = _context.Chamados.FirstOrDefault(c => c.Id == id);
            if (chamado == null)
                return NotFound("Chamado não encontrado.");

            // 🔹 Atualiza status se enviado
            if (dto.TryGetProperty("Status", out JsonElement statusElement))
            {
                string statusNome = statusElement.GetString() ?? "";
                var status = _context.Statuses.FirstOrDefault(s => s.Nome == statusNome);
                if (status == null)
                    return BadRequest($"Status '{statusNome}' não encontrado.");
                chamado.StatusId = status.Id;
            }

            // 🔹 Atribui técnico se enviado
            if (dto.TryGetProperty("TecnicoId", out JsonElement tecnicoElement) &&
                tecnicoElement.ValueKind == JsonValueKind.Number)
            {
                chamado.ResponsavelId = tecnicoElement.GetInt32();
            }

            // 🔹 Adiciona feedback se enviado
            if (dto.TryGetProperty("Feedback", out JsonElement feedbackElement))
            {
                string textoFeedback = feedbackElement.GetString() ?? "";
                if (!string.IsNullOrWhiteSpace(textoFeedback))
                {
                    chamado.Descricao += $"\n\n🧰 Feedback Técnico ({DateTime.Now:dd/MM/yyyy HH:mm}): {textoFeedback}";
                }
            }

            chamado.AtualizadoEm = DateTime.Now;
            _context.SaveChanges();

            return Ok(new
            {
                mensagem = "Chamado atualizado com sucesso!",
                chamado.Id,
                novoStatus = _context.Statuses.FirstOrDefault(s => s.Id == chamado.StatusId)?.Nome,
                tecnico = chamado.ResponsavelId
            });
        }
    }
}
