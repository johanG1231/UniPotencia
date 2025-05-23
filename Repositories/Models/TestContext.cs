using System;
using System.Collections.Generic;
using Backend_PotenciaPC.Services;
using Microsoft.EntityFrameworkCore;
using Backend_PotenciaPC.Repositories.Models;

namespace Backend_PotenciaPC.Repositories.Models
{
    public partial class TestContext : DbContext
    {
        public TestContext(DbContextOptions<TestContext> options) : base(options) { }

        public DbSet<User> Usuarios { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Descarga> Descargas { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=JOHAN\\SQLEXPRESS;Database=PotenciaPC;Integrated Security=True;TrustServerCertificate=True", opt =>
                {
                    opt.UseRelationalNulls();
                });
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Audit>(entity =>
            {
                entity.HasKey(e => e.IdAudit).HasName("PK_AUDIT_1");

                entity.ToTable("AUDIT");

                entity.Property(e => e.IdAudit).HasColumnName("id_audit");
                entity.Property(e => e.Acction)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("acction");
                entity.Property(e => e.Date)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("date");
                entity.Property(e => e.IdUser).HasColumnName("id_user");
                entity.Property(e => e.ViewAction)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("view_action");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Nombre).HasColumnName("Nombre").HasMaxLength(50).IsUnicode(true);
            });

            modelBuilder.Entity<User>(entity =>
            {
                // 🟡 Aquí especificamos que la tabla tiene un trigger:
                entity.ToTable("Usuarios", tb => tb.HasTrigger("Trigger_Insert_Usuarios")); // ⬅️ Cambia el nombre si el trigger se llama distinto

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .HasColumnName("id")
                      .ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre).HasColumnName("nombre").HasMaxLength(100).IsUnicode(true);
                entity.Property(e => e.Correo).HasColumnName("correo").HasMaxLength(100).IsUnicode(true);
                entity.Property(e => e.Contraseña).HasColumnName("contraseña").HasMaxLength(200).IsUnicode(true);
                entity.Property(e => e.RolId).HasColumnName("rolid");
                entity.Property(e => e.FechaCreacion).HasColumnName("fechacreacion").HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.Activo).HasColumnName("activo").HasDefaultValue(true);
                entity.Property(e => e.Confirmado).HasColumnName("confirmado").HasDefaultValue(false);
                entity.Property(e => e.TokenConfirmacion).HasColumnName("TokenConfirmacion").HasMaxLength(200);

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RolId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Usuarios_Roles");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.ToTable("Productos", tb => tb.HasTrigger("Trigger_Insert_Productos")); // 🔁 Asegúrate que el nombre del trigger sea el correcto

                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id").ValueGeneratedOnAdd();

                entity.Property(e => e.Nombre).HasMaxLength(100).IsUnicode(true);
                entity.Property(e => e.Precio).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Descripcion).HasMaxLength(500).IsUnicode(true);
                entity.Property(e => e.ArchivoUrl).HasMaxLength(200).IsUnicode(true);
            });

            OnModelCreatingPartial(modelBuilder);
            
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
        


    }
}
