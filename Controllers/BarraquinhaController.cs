using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueDiversao.Context;
using ParqueDiversao.Entities;
using ParqueDiversao.Services;

namespace ParqueDiversao.Controllers;
[ApiController]
[Route("[controller]")]
public class BarraquinhaController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly SetorService _setorService;

    public BarraquinhaController(ApplicationDbContext context, SetorService setorService)
    {
        _context = context;
        _setorService = setorService;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Barraquinha>>> ObterTodos()
    {
        var barraquinhas = await _context.Barraquinhas
        .Select(barraquinha => new
        {
            barraquinha.Id,
            barraquinha.Nome,
            barraquinha.ValorObtido,
            barraquinha.Custo,
            Setor = new
            {
                barraquinha.Setor.Id,
                barraquinha.Setor.Nome,
                QtdAtracoes = _setorService.ContagemQtdAtracoes(),
                LucroTotal = _setorService.SomaValorObtidoTotal(),
                QtdAtracoesQuebradas = _setorService.ContagemQtdAtracoesQuebradas(),
                QtdAtracoesAtivas = _setorService.ContagemQtdAtracoesAtivas(),
                Atracao = barraquinha.Setor.Atracoes.Select(atracao => new
                {
                    atracao.Id,
                    atracao.Nome,
                    atracao.Ativa,
                    atracao.Preco,
                    atracao.UltimaManutencao,
                    infoAtracao = atracao.InfoAtracao != null ? new
                    {
                        atracao.InfoAtracao.Id,
                        atracao.InfoAtracao.EntradasVendidas,
                        atracao.InfoAtracao.ValorObtido,
                        atracao.InfoAtracao.Custo
                    } : null
                })
            }
        }).ToListAsync();

        return Ok(barraquinhas);
    }

    [HttpPost("adicionar-barraquinha")]
    public async Task<ActionResult<Barraquinha>> AdicionarBarraquinha([FromBody] Barraquinha model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var novaBarraquinha = new Barraquinha
            {
                Nome = model.Nome,
                ValorObtido = model.ValorObtido,
                Custo = model.Custo,
                SetorId = model.SetorId
            };

            if (novaBarraquinha == null)
                return BadRequest("Nova barraquinha tem valores inválidos.");

            _context.Barraquinhas.Add(novaBarraquinha);
            await _context.SaveChangesAsync();

            return Ok(model);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut("atualizar-barraquinha/{id:int}")]
    public async Task<ActionResult<Barraquinha>> AtualizarBarraquinha(int id, Barraquinha model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var barraquinha = await _context.Barraquinhas.FindAsync(id);

            if (barraquinha == null)
                return BadRequest($"Barraquinha de ID {id} não existe.");

            barraquinha.Nome = model.Nome;
            barraquinha.ValorObtido = model.ValorObtido;
            barraquinha.Custo = model.Custo;

            _context.Barraquinhas.Update(barraquinha);
            await _context.SaveChangesAsync();

            return Ok(model);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("deletar-barraquinha/{id}")]
    public async Task<ActionResult> DeletarBarraquinha(int id)
    {
        try
        {
            var barraquinha = await _context.Barraquinhas.FindAsync(id);

            if (barraquinha == null)
                return BadRequest($"Barraquinha de ID {id} não existe.");

            _context.Barraquinhas.Remove(barraquinha);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {

            throw;
        }
    }
}