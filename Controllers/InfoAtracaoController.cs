using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParqueDiversao.Context;
using ParqueDiversao.Entities;
using ParqueDiversao.Services;

namespace ParqueDiversao.Controllers;
[ApiController]
[Route("[controller]")]
public class InfoAtracaoController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly SetorService _setorService;

    public InfoAtracaoController(ApplicationDbContext context, SetorService setorService)
    {
        _context = context;
        _setorService = setorService;
    }



    [HttpGet()]
    public async Task<ActionResult<IEnumerable<InfoAtracao>>> ObterTodos()
    {
        try
        {
            var getAll = await _context.InfosAtracao.Select(infoAtracao => new
            {

                infoAtracao.Id,
                infoAtracao.EntradasVendidas,
                infoAtracao.ValorObtido,
                infoAtracao.Custo,
                Atracao = new
                {
                    infoAtracao.Atracao.Id,
                    infoAtracao.Atracao.Nome,
                    infoAtracao.Atracao.Ativa,
                    infoAtracao.Atracao.Preco,
                    infoAtracao.Atracao.UltimaManutencao,
                    Setor = new
                    {
                        infoAtracao.Atracao.Setor.Id,
                        infoAtracao.Atracao.Setor.Nome,
                        QtdAtracoes = _setorService.ContagemQtdAtracoes(),
                        LucroTotal = _setorService.SomaValorObtidoTotal(),
                        QtdAtracoesQuebradas = _setorService.ContagemQtdAtracoesQuebradas(),
                        QtdAtracoesAtivas = _setorService.ContagemQtdAtracoesAtivas()
                    }
                }
            }).ToListAsync();

            if (getAll == null)
                return BadRequest("Houve um erro ao buscar as atracoes!");
            //adicionar mais validacoes dps

            return Ok(getAll);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost("adicionar-info-atracao")]
    public async Task<ActionResult<InfoAtracao>> AdicionarInfoAtracao([FromBody] InfoAtracao infoAtracaoModel)
    {
        try
        {
            if (infoAtracaoModel == null)
                return BadRequest("Houve um erro ao adicionar uma nova info dessa atracao!");

            var newInfoAtracao = new InfoAtracao
            {
                EntradasVendidas = infoAtracaoModel.EntradasVendidas,
                Custo = infoAtracaoModel.Custo,
                AtracaoId = infoAtracaoModel.AtracaoId
            };

            if (newInfoAtracao == null)
                return BadRequest("Nova InfoAtração é inválida");

            newInfoAtracao.ValorObtido = await SomarGanhos();

            await _context.InfosAtracao.AddAsync(newInfoAtracao);
            await _context.SaveChangesAsync();

            return Ok(newInfoAtracao);
        }
        catch (Exception)
        {

            throw;
        }
    }

    [HttpPut("editar-info-atracao/{id:int}")]
    public async Task<ActionResult<InfoAtracao>> EditarInfoAtracao(InfoAtracao infoAtracaoModel, int id)
    {
        try
        {
            var getInfoAtracao = await _context.InfosAtracao.FindAsync(id);

            if (getInfoAtracao == null)
                return BadRequest($"A info da Atracao nao existe!");
            //if(//VALIDAR ATRACAO)
            //return BadRequest("Para criar uma INFO ATRACAO é NECESSARIO uma ATRACAO!");
            //adicionar mais validacoes dps

            getInfoAtracao.EntradasVendidas = infoAtracaoModel.EntradasVendidas;
            getInfoAtracao.ValorObtido = infoAtracaoModel.ValorObtido;
            getInfoAtracao.Custo = infoAtracaoModel.Custo;

            _context.InfosAtracao.Update(getInfoAtracao);
            await _context.SaveChangesAsync();

            return Ok(getInfoAtracao);
        }
        catch (Exception)
        {

            throw;
        }
    }

    [HttpDelete("deletar-info-atracao-existente/{id}")]
    public async Task<ActionResult> DeletarAtracao(int id)
    {
        try
        {
            var getInfoAtracao = await _context.InfosAtracao.FindAsync(id);

            if (getInfoAtracao == null)
                return BadRequest($"A info dessa atracao nao existe!");
            //adicionar mais validacoes dps

            _context.InfosAtracao.Remove(getInfoAtracao);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception)
        {

            throw;
        }
    }

    private async Task<decimal> SomarGanhos()
    {
        var infoAtracao = await _context.InfosAtracao
                                        .Include(e => e.Atracao)
                                        .FirstOrDefaultAsync();

        if (infoAtracao == null)
        {
            // Retorna 0 ou outra ação apropriada caso o objeto infoAtracao seja nulo
            return 0;
        }

        decimal somar = (decimal)infoAtracao.EntradasVendidas * infoAtracao.Atracao.Preco;

        return somar;
    }
}