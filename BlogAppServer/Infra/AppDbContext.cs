using BlogApp.Server.Domain.Modelos;
using BlogApp.Server.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Server.Infra
{
    public class AppDbContext : IdentityDbContext<UsuarioAplicacion>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected string Schema => "BlogApp";

        public DbSet<Blog> Blog { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.Entity<UsuarioAplicacion>(entity =>
            {
                entity.Property(e => e.RefreshToken).HasMaxLength(256);
                entity.Property(e => e.RefreshTokenExpiryTime).IsRequired();
            });

            modelBuilder.Entity<Blog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired();
                entity.Property(e => e.Contenido).IsRequired().HasColumnType("TEXT");
                entity.Property(e => e.Link);
                entity.Property(e => e.AutorId).IsRequired();
                entity.HasOne(r => r.Autor).WithMany().HasForeignKey(fk => fk.AutorId).OnDelete(DeleteBehavior.Restrict);
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
