using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebSRVC_Core.Modelo
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ImagenesPersona> ImagenesPersona { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\sqlexpress;Database=ReconocimientoFacial;Trusted_Connection=True;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ImagenesPersona>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Fecha)
                    .HasColumnName("fecha")
                    .HasColumnType("datetime");

                entity.Property(e => e.Foto)
                    .IsRequired()
                    .HasColumnName("foto");

                entity.Property(e => e.Fotocompleta).HasColumnName("fotocompleta");

                entity.Property(e => e.PersonaId).HasColumnName("personaID");

                entity.HasOne(d => d.Persona)
                    .WithMany(p => p.ImagenesPersona)
                    .HasForeignKey(d => d.PersonaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ImagenesP__perso__38996AB5");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Edad).HasColumnName("edad");

                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasColumnName("nombre")
                    .HasMaxLength(200);

                entity.Property(e => e.TrainData).HasColumnName("trainData");
            });
        }
    }
}
