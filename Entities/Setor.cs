namespace ParqueDiversao.Entities;
public class Setor
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int? QtdAtracoes { get; set; }
    public decimal? LucroTotal { get; set; }
    public int? QtdAtracoesQuebradas { get; set; }
    public int? QtdAtracoesAtivas { get; set; }
    public virtual IEnumerable<Atracao>? Atracoes { get; set; }
    public virtual IEnumerable<Barraquinha>? Barraquinhas { get; set; }
}
