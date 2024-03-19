namespace ParqueDiversao.Entities;
public class Barraquinha
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public decimal? ValorObtido { get; set; }
    public decimal? Custo { get; set; }
    public int SetorId { get; set; }
    public virtual Setor? Setor { get; set; }
}
