using Microsoft.EntityFrameworkCore;
using ParqueDiversao.Context;

namespace ParqueDiversao.Services;
public class SetorService
{
    private readonly ApplicationDbContext _context;
    public SetorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public int ContagemQtdAtracoes()
    {
        try
        {
            var setor = _context.Setores
                .Include(e => e.Atracoes)
                .FirstOrDefaultAsync().GetAwaiter().GetResult();

            if (setor == null)
                return 0;

            int qtdAtracoes = setor.Atracoes.Count();

            return qtdAtracoes;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public decimal SomaValorObtidoTotal()
    {
        try
        {
            var atracao = _context.Atracoes
                .Include(e => e.InfoAtracao)
                .FirstOrDefaultAsync().GetAwaiter().GetResult();

            var barraquinha = _context.Barraquinhas
                .FirstOrDefaultAsync().GetAwaiter().GetResult();

            if (atracao == null && barraquinha == null)
                return 0;

            decimal valorTotal = 0;
            decimal valorObtidoAtracao = 0;
            decimal valorObtidoBarraquinha = 0;

            valorObtidoAtracao = (atracao.Preco * (decimal)atracao.InfoAtracao.EntradasVendidas) - (decimal)atracao.InfoAtracao.Custo;
            valorObtidoBarraquinha = (decimal)barraquinha.ValorObtido - (decimal)barraquinha.Custo;

            valorTotal = valorObtidoAtracao + valorObtidoBarraquinha;

            return valorTotal;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int ContagemQtdAtracoesQuebradas()
    {
        try
        {
            var setor =  _context.Setores
                .Include(e => e.Atracoes)
                .FirstOrDefaultAsync().GetAwaiter().GetResult();

            int qtdAtracoesQuebradas = 0;
            qtdAtracoesQuebradas = setor.Atracoes.Count(e => !e.Ativa);

            return qtdAtracoesQuebradas;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int  ContagemQtdAtracoesAtivas()
    {
        try
        {
            var setor =  _context.Setores
                .Include(e => e.Atracoes)
                .FirstOrDefaultAsync().GetAwaiter().GetResult();

            int qtdAtracoesAtivas = 0;
            qtdAtracoesAtivas = setor.Atracoes.Count(e => e.Ativa);

            return qtdAtracoesAtivas;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
