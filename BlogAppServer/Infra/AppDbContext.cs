using BlogApp.Server.Domain.Modelos;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(Schema);

            modelBuilder.Entity<UsuarioAplicacion>(entity =>
            {
                entity.Property(e => e.RefreshToken).HasMaxLength(256);
                entity.Property(e => e.RefreshTokenExpiryTime).IsRequired();
            });
            
            base.OnModelCreating(modelBuilder);
        }
    }
}
