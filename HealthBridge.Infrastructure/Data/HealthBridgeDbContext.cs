
using HealthBridge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HealthBridge.Infrastructure.Data;

public class HealthBridgeDbContext : DbContext
{
    public HealthBridgeDbContext(
        DbContextOptions<HealthBridgeDbContext> options)
        : base(options)
    {
    }

    public DbSet<Paciente> Pacientes => Set<Paciente>();

    public DbSet<Consulta> Consultas => Set<Consulta>();

    public DbSet<ResultadoLaboratorial> ResultadosLaboratoriais
        => Set<ResultadoLaboratorial>();

    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.NumeroProcesso)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(p => p.NumeroProcesso)
                .IsUnique();

            entity.Property(p => p.NomeCompleto)
                .IsRequired()
                .HasMaxLength(200);
        });

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.NumeroConsulta)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(c => c.Paciente)
                .WithMany()
                .HasForeignKey(c => c.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ResultadoLaboratorial>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.Property(r => r.CodigoAnalise)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(r => r.Valor)
                .HasPrecision(18, 4);

            entity.HasOne(r => r.Paciente)
                .WithMany()
                .HasForeignKey(r => r.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(r => r.Consulta)
                .WithMany()
                .HasForeignKey(r => r.ConsultaId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
