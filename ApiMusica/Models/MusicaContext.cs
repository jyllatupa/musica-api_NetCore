using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ApiMusica.Models;

public partial class MusicaContext : DbContext
{
    public MusicaContext()
    {
    }

    public MusicaContext(DbContextOptions<MusicaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Canciones> Canciones { get; set; }

    public virtual DbSet<Cantantes> Cantantes { get; set; }

    public virtual DbSet<DetPlaylists> DetPlaylists { get; set; }

    public virtual DbSet<Favoritos> Favoritos { get; set; }

    public virtual DbSet<Generos> Generos { get; set; }

    public virtual DbSet<Playlists> Playlists { get; set; }

    public virtual DbSet<Reproduciones> Reproduciones { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Canciones>(entity =>
        {
            entity.HasKey(e => e.Codcancion).HasName("PK__Cancione__3D6D09911B327953");

            entity.Property(e => e.Codcancion).HasColumnName("codcancion");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.Codcantante).HasColumnName("codcantante");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Link)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("link");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Rpath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("RPath");

            entity.HasOne(d => d.oCodcantante).WithMany(p => p.Canciones)
                .HasForeignKey(d => d.Codcantante)
                .HasConstraintName("FK__Canciones__codca__29572725");
        });

        modelBuilder.Entity<Cantantes>(entity =>
        {
            entity.HasKey(e => e.Codcantante).HasName("PK__Cantante__8F1A264089D48002");

            entity.Property(e => e.Codcantante).HasColumnName("codcantante");
            entity.Property(e => e.Codgenero).HasColumnName("codgenero");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");

            entity.HasOne(d => d.oCodgenero).WithMany(p => p.Cantantes)
                .HasForeignKey(d => d.Codgenero)
                .HasConstraintName("FK__Cantantes__codge__267ABA7A");
        });

        modelBuilder.Entity<DetPlaylists>(entity =>
        {
            entity.HasKey(e => e.Coddetplay).HasName("PK__DetPlayl__90FEDAD10CBBE9D3");

            entity.ToTable("DetPlaylist");

            entity.Property(e => e.Coddetplay).HasColumnName("coddetplay");
            entity.Property(e => e.Codcancion).HasColumnName("codcancion");
            entity.Property(e => e.Codplaylist).HasColumnName("codplaylist");

            entity.HasOne(d => d.oCodcancion).WithMany(p => p.DetPlaylists)
                .HasForeignKey(d => d.Codcancion)
                .HasConstraintName("FK__DetPlayli__codca__3E52440B");

            entity.HasOne(d => d.CodplaylistNavigation).WithMany(p => p.DetPlaylists)
                .HasForeignKey(d => d.Codplaylist)
                .HasConstraintName("FK__DetPlayli__codpl__3D5E1FD2");
        });

        modelBuilder.Entity<Favoritos>(entity =>
        {
            entity.HasKey(e => e.Codfavorito).HasName("PK__Favorito__A440C4B07C426E31");

            entity.Property(e => e.Codfavorito).HasColumnName("codfavorito");
            entity.Property(e => e.Codcancion).HasColumnName("codcancion");
            entity.Property(e => e.Codusuario).HasColumnName("codusuario");

            entity.HasOne(d => d.oCodcancion).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.Codcancion)
                .HasConstraintName("FK__Favoritos__codca__32E0915F");

            entity.HasOne(d => d.oCodusuario).WithMany(p => p.Favoritos)
                .HasForeignKey(d => d.Codusuario)
                .HasConstraintName("FK__Favoritos__codus__31EC6D26");
        });

        modelBuilder.Entity<Generos>(entity =>
        {
            entity.HasKey(e => e.Codgenero).HasName("PK__Genero__D78E07EA809D56BD");

            entity.ToTable("Genero");

            entity.Property(e => e.Codgenero).HasColumnName("codgenero");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
        });

        modelBuilder.Entity<Playlists>(entity =>
        {
            entity.HasKey(e => e.Codplaylist).HasName("PK__Playlist__B99569DFF9AB5324");

            entity.ToTable("Playlist");

            entity.Property(e => e.Codplaylist).HasColumnName("codplaylist");
            entity.Property(e => e.Codusuario).HasColumnName("codusuario");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("descripcion");
            entity.Property(e => e.Fechacreacion)
                .HasColumnType("datetime")
                .HasColumnName("fechacreacion");

            entity.HasOne(d => d.oCodusuario).WithMany(p => p.Playlists)
                .HasForeignKey(d => d.Codusuario)
                .HasConstraintName("FK__Playlist__codusu__3A81B327");
        });

        modelBuilder.Entity<Reproduciones>(entity =>
        {
            entity.HasKey(e => e.Codrepro).HasName("PK__Reproduc__03E4BB71C9A69EBA");

            entity.Property(e => e.Codrepro).HasColumnName("codrepro");
            entity.Property(e => e.Codcancion).HasColumnName("codcancion");
            entity.Property(e => e.Codusuario).HasColumnName("codusuario");
            entity.Property(e => e.Countrepro).HasColumnName("countrepro");

            entity.HasOne(d => d.oCodcancion).WithMany(p => p.Reproduciones)
                .HasForeignKey(d => d.Codcancion)
                .HasConstraintName("FK__Reproduci__codca__36B12243");

            entity.HasOne(d => d.oCodusuario).WithMany(p => p.Reproduciones)
                .HasForeignKey(d => d.Codusuario)
                .HasConstraintName("FK__Reproduci__codus__35BCFE0A");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.Codusuario).HasName("PK__Usuario__69EF01DD1D4DD167");

            entity.ToTable("Usuario");

            entity.Property(e => e.Codusuario).HasColumnName("codusuario");
            entity.Property(e => e.Clave)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Estado).HasColumnName("estado");
            entity.Property(e => e.Fecharegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("fecharegistro");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Nomusuario)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nomusuario");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
