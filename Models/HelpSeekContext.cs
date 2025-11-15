using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace HelpSeek.API.Models;

public partial class HelpSeekContext : DbContext
{
    public HelpSeekContext()
    {
    }

    public HelpSeekContext(DbContextOptions<HelpSeekContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Chamado> Chamados { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Interaco> Interacoes { get; set; }

    public virtual DbSet<Prioridade> Prioridades { get; set; }

    public virtual DbSet<Sistema> Sistemas { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost\\MSSQLSERVER01;Database=HelpSeek;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07DAEA144D");

            entity.Property(e => e.Nome).HasMaxLength(150);
        });

        modelBuilder.Entity<Chamado>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Chamados__3214EC07BA7F942E");

            entity.Property(e => e.CriadoEm).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.EmailUsuario).HasMaxLength(200);
            entity.Property(e => e.Titulo).HasMaxLength(250);

            entity.HasOne(d => d.Categoria).WithMany(p => p.Chamados)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chamados_Categorias");

            entity.HasOne(d => d.Prioridade).WithMany(p => p.Chamados)
                .HasForeignKey(d => d.PrioridadeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chamados_Prioridades");

            entity.HasOne(d => d.Responsavel).WithMany(p => p.ChamadoResponsavels)
                .HasForeignKey(d => d.ResponsavelId)
                .HasConstraintName("FK_Chamados_Responsavel");

            entity.HasOne(d => d.SistemaOrigem).WithMany(p => p.Chamados)
                .HasForeignKey(d => d.SistemaOrigemId)
                .HasConstraintName("FK_Chamados_Sistemas");

            entity.HasOne(d => d.Status).WithMany(p => p.Chamados)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chamados_Status");

            entity.HasOne(d => d.Usuario).WithMany(p => p.ChamadoUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Chamados_Usuarios");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Feedback__3214EC07BD85F2B7");

            entity.Property(e => e.CriadoEm).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.Chamado).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.ChamadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Feedbacks_Chamados");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Feedbacks_Usuarios");
        });

        modelBuilder.Entity<Interaco>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Interaco__3214EC071681A063");

            entity.Property(e => e.CriadoEm).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Origem).HasMaxLength(50);

            entity.HasOne(d => d.Chamado).WithMany(p => p.Interacos)
                .HasForeignKey(d => d.ChamadoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Interacoes_Chamados");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Interacos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Interacoes_Usuarios");
        });

        modelBuilder.Entity<Prioridade>(entity =>
        {
            entity.HasKey(e => e.PrioridadeId).HasName("PK__Priorida__3DDF860882EDE296");

            entity.Property(e => e.Nivel).HasMaxLength(100);
        });

        modelBuilder.Entity<Sistema>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Sistemas__3214EC07A7DB9813");

            entity.Property(e => e.Nome).HasMaxLength(150);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Status__3214EC07F6D84F6B");

            entity.ToTable("Status");

            entity.Property(e => e.Nome).HasMaxLength(100);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Usuarios__3214EC0739784C07");

            entity.Property(e => e.Departamento).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(200);
            entity.Property(e => e.Nome).HasMaxLength(200);
            entity.Property(e => e.Papel).HasMaxLength(50);
            entity.Property(e => e.SenhaHash).HasMaxLength(200);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
