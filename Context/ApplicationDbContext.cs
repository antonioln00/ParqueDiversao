using Microsoft.EntityFrameworkCore;
using ParqueDiversao.Entities;

namespace ParqueDiversao.Context;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Setor> Setores { get; set; }
    public DbSet<Atracao> Atracoes { get; set; }
    public DbSet<InfoAtracao> InfosAtracao { get; set; }
    public DbSet<Barraquinha> Barraquinhas { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Setor>().HasKey(e => e.Id);
        modelBuilder.Entity<Atracao>().HasKey(e => e.Id);
        modelBuilder.Entity<InfoAtracao>().HasKey(e => e.Id);
        modelBuilder.Entity<Barraquinha>().HasKey(e => e.Id);

        modelBuilder.Entity<Setor>().Property(e => e.LucroTotal).HasPrecision(18, 5);
        modelBuilder.Entity<Atracao>().Property(e => e.Preco).HasPrecision(18, 5);
        modelBuilder.Entity<InfoAtracao>().Property(e => e.ValorObtido).HasPrecision(18, 5);
        modelBuilder.Entity<InfoAtracao>().Property(e => e.Custo).HasPrecision(18, 5);
        modelBuilder.Entity<Barraquinha>().Property(e => e.ValorObtido).HasPrecision(18, 5);
        modelBuilder.Entity<Barraquinha>().Property(e => e.Custo).HasPrecision(18, 5);


        modelBuilder
            .Entity<Barraquinha>()
            .HasOne(e => e.Setor)
            .WithMany(e => e.Barraquinhas)
            .HasForeignKey(e => e.SetorId)
            .IsRequired();

        modelBuilder
        .Entity<Atracao>()
        .HasOne(e => e.InfoAtracao) // A Atração tem uma InfoAtração
        .WithOne(e => e.Atracao) // A InfoAtração tem uma Atração
        .HasForeignKey<InfoAtracao>(e => e.AtracaoId) // Chave estrangeira na tabela InfoAtração
        .IsRequired();

        modelBuilder
            .Entity<Atracao>()
            .HasOne(e => e.Setor)
            .WithMany(e => e.Atracoes)
            .HasForeignKey(e => e.SetorId)
            .IsRequired();
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer("DefaultConnection");

        base.OnConfiguring(optionsBuilder);
    }
}
