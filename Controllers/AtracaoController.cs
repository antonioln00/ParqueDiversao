using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueDiversao.Context;
using ParqueDiversao.Entities;
using ParqueDiversao.Services;

namespace ParqueDiversao.Controllers;
[ApiController]
[Route("[controller]")]
public class AtracaoController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly SetorService _setorService;

    public AtracaoController(ApplicationDbContext context, SetorService setorService)
    {
        _context = context;
        _setorService = setorService;
    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Atracao>>> ObterTodos()
    {
        try
        {
            var getAll = await _context.Atracoes.Select(atracao => new
            {
                atracao.Id,
                atracao.Nome,
                atracao.Ativa,
                atracao.Preco,
                atracao.UltimaManutencao,
                Setor = new
                {
                    atracao.Setor.Id,
                    atracao.Setor.Nome,
                    QtdAtracoes = _setorService.ContagemQtdAtracoes(),
                    LucroTotal = _setorService.SomaValorObtidoTotal(),
                    QtdAtracoesQuebradas = _setorService.ContagemQtdAtracoesQuebradas(),
                    QtdAtracoesAtivas = _setorService.ContagemQtdAtracoesAtivas(),
                    barraquinha = atracao.Setor.Barraquinhas.Select(barraquinha => new
                    {
                        barraquinha.Id,
                        barraquinha.Nome,
                        barraquinha.ValorObtido,
                        barraquinha.Custo
                    })
                },
                InfoAtracao = new
                {
                    atracao.InfoAtracao.Id,
                    atracao.InfoAtracao.EntradasVendidas,
                    atracao.InfoAtracao.ValorObtido,
                    atracao.InfoAtracao.Custo
                }
            }).ToListAsync();

            if (getAll == null)
                return BadRequest("Houve um erro ao buscar as atracoes!");

            return Ok(getAll);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost("adicionar-atracao")]
    public async Task<ActionResult<Atracao>> AdicionarAtracao([FromBody] Atracao model)
    {
        try
        {
            if (model == null)
                return BadRequest("Dados inseridos inválidos.");

            var newAtracao = new Atracao
            {
                Nome = model.Nome,
                Ativa = model.Ativa,
                Preco = model.Preco,
                UltimaManutencao = model.UltimaManutencao,
                SetorId = model.SetorId
            };

            _context.Atracoes.Add(newAtracao);
            await _context.SaveChangesAsync();

            return Ok(newAtracao);
        }
        catch (Exception)
        {

            throw;
        }
    }

    [HttpPut("editar-atracao-existente/{id:int}")]
    public async Task<ActionResult<Atracao>> EditarAtracao(Atracao atracaoModel, int id)
    {
        try
        {
            var getAtracao = await _context.Atracoes.FindAsync(id);

            if (getAtracao == null)
                return BadRequest($"Atracao de Id {id} nao existe!");
            //adicionar mais validacoes dps

            getAtracao.Nome = atracaoModel.Nome;
            getAtracao.Ativa = atracaoModel.Ativa;
            getAtracao.Preco = atracaoModel.Preco;
            getAtracao.UltimaManutencao = atracaoModel.UltimaManutencao;


            _context.Atracoes.Update(getAtracao);
            await _context.SaveChangesAsync();

            return Ok(getAtracao);
        }
        catch (Exception)
        {

            throw;
        }
    }

    [HttpDelete("deletar-atracao-existente/{id}")]
    public async Task<ActionResult> DeletarAtracao(int id)
    {
        try
        {
            var getAtracao = await _context.Atracoes.FindAsync(id);

            if (getAtracao == null)
                return BadRequest($"Atracao de Id {id} nao existe!");
            //adicionar mais validacoes dps

            _context.Atracoes.Remove(getAtracao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {

            throw;
        }
    }
}