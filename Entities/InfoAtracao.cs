namespace ParqueDiversao.Entities;
public class InfoAtracao
{
    public int Id { get; set; }
    public int? EntradasVendidas { get; set; }
    public decimal? ValorObtido { get; set; }
    public decimal? Custo { get; set; }
    public int AtracaoId { get; set; }
    public virtual Atracao? Atracao { get; set; }
}
