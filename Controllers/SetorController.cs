using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueDiversao.Context;
using ParqueDiversao.Entities;
using ParqueDiversao.Services;

namespace ParqueDiversao.Controllers;
[ApiController]
[Route("[controller]")]
public class SetorController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly SetorService _setorService;

    public SetorController(ApplicationDbContext context, SetorService setorService)
    {
        _context = context;
        _setorService = setorService;

    }

    [HttpGet()]
    public async Task<ActionResult<IEnumerable<Setor>>> ObterTodos() =>
        Ok(await _context.Setores.Select(setor => new
        {
            setor.Id,
            setor.Nome,
            QtdAtracoes = _setorService.ContagemQtdAtracoes(),
            LucroTotal = _setorService.SomaValorObtidoTotal(),
            QtdAtracoesQuebradas = _setorService.ContagemQtdAtracoesQuebradas(),
            QtdAtracoesAtivas = _setorService.ContagemQtdAtracoesAtivas(),
            Atracao = setor.Atracoes.Select(atracao => new
            {
                atracao.Id,
                atracao.Nome,
                atracao.Ativa,
                atracao.Preco,
                atracao.UltimaManutencao,
                InfoAtracao = new
                {
                    atracao.InfoAtracao.Id,
                    atracao.InfoAtracao.EntradasVendidas,
                    atracao.InfoAtracao.ValorObtido,
                    atracao.InfoAtracao.Custo,
                }
            })
        }).ToListAsync());

    [HttpPost("adicionar-setor")]
    public async Task<ActionResult<Setor>> AdicionarSetor(string nome)
    {
        try
        {
            if (string.IsNullOrEmpty(nome))
                return BadRequest("Preencha o campo nome.");

            var novoSetor = new Setor
            {
                Nome = nome,
            };

            _context.Setores.Add(novoSetor);
            await _context.SaveChangesAsync();

            return Ok(novoSetor);

        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPut("atualizar-setor/{id:int}")]
    public async Task<ActionResult<Setor>> AtualizarSetor(int id, string nome)
    {
        try
        {
            if (nome == null)
                return BadRequest("O nome é obrigatório para atualizar o setor.");

            var setor = await _context.Setores.FindAsync(id);

            if (setor == null)
                return BadRequest($"O setor de ID {id} não existe.");

            setor.Nome = nome;

            _context.Setores.Update(setor);
            await _context.SaveChangesAsync();

            return Ok(setor);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpDelete("deletar-setor/{id}")]
    public async Task<ActionResult> DeletarSetor(int id)
    {
        try
        {
            var setor = await _context.Setores.Include(e => e.Atracoes).Include(e => e.Barraquinhas).FirstOrDefaultAsync(e => e.Id == id);

            if (setor == null)
                return BadRequest($"O setor de ID {id} não existe.");

            if (setor.Atracoes.Any())
                return BadRequest("Não foi possível deletar setor porque há atrações atreladas a ele.");


            if (setor.Barraquinhas.Any())
                return BadRequest("Não foi possível deletar setor porque há barraquinhas atreladas a ele.");

            _context.Setores.Remove(setor);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {
            throw;
        }
    }
}