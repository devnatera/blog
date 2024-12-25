using Microsoft.AspNetCore.Identity;

namespace BlogApp.Server.Domain.Modelos
{
    public class UsuarioAplicacion : IdentityUser
    {
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
