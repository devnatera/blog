using BlogApp.Server.App.Services;
using BlogApp.Server.Config;
using BlogApp.Server.Domain.Modelos;
using BlogApp.Shared.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace BlogApp.Server.App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UsuarioAplicacion> _userManager;
        private readonly ITokenService _tokenService;
        private readonly JwtConfig _jwtConfig;

        public AuthController(UserManager<UsuarioAplicacion> userManager, ITokenService tokenService, IOptions<JwtConfig> jwtSettings)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _jwtConfig = jwtSettings.Value;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UsuarioDto usuarioDto)
        {
            var usuario = new UsuarioAplicacion
            {
                UserName = usuarioDto.Nombre,
                Email = usuarioDto.Correo,
            };
            var result = await _userManager.CreateAsync(usuario, usuarioDto.Clave);

            if (result.Succeeded)
            {
                return Ok(new { Message = "Usuario registrado!" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioDto usuarioDto)
        {
            var usuario = await _userManager.FindByEmailAsync(usuarioDto.Correo);
            if (usuario != null && await _userManager.CheckPasswordAsync(usuario, usuarioDto.Clave))
            {
                var token = _tokenService.GenerateToken(usuario.Id);
                var refreshToken = _tokenService.GenerateRefreshToken();

                usuario.RefreshToken = refreshToken;
                usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenLifetimeDays);
                await _userManager.UpdateAsync(usuario);

                return Ok(new { Token = token, RefreshToken = refreshToken });
            }

            return Unauthorized();
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] UsuarioDto usuarioDto)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(usuarioDto.Token);
            var usuarioId = principal.FindFirstValue(ClaimTypes.NameIdentifier);

            var usuario = await _userManager.FindByIdAsync(usuarioId);
            if (usuario == null || usuario.RefreshToken != usuarioDto.RefreshToken || usuario.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return Unauthorized();
            }

            var token = _tokenService.GenerateToken(usuario.Id);
            var refreshToken = _tokenService.GenerateRefreshToken();

            usuario.RefreshToken = refreshToken;
            usuario.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenLifetimeDays);
            await _userManager.UpdateAsync(usuario);

            return Ok(new { Token = token, RefreshToken = refreshToken });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var usuario = await _userManager.GetUserAsync(User);
            if (usuario != null)
            {
                usuario.RefreshToken = string.Empty;
                await _userManager.UpdateAsync(usuario);
            }

            return NoContent();
        }
    }
}
