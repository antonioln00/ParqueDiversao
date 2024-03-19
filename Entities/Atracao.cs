namespace ParqueDiversao.Entities;
public class Atracao
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public bool Ativa { get; set; }
    public decimal Preco { get; set; }
    public DateTime? UltimaManutencao { get; set; }
    public int SetorId { get; set; }
    public virtual Setor? Setor { get; set; }
    public virtual InfoAtracao? InfoAtracao { get; set; }
}
