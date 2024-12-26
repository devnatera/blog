using BlogApp.Server.Domain.Modelos;
using BlogApp.Server.Infra;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BlogApp.Server.Config
{
    public static class AuthConfig
    {
        public static void ConfigureAuth(this IServiceCollection services, IConfiguration config)
        {
            var jwtConfig = config.GetSection("JwtConfig").Get<JwtConfig>() ?? new();

            services.AddIdentity<UsuarioAplicacion, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false, // Habilita la validación de tiempo de vida
                        ValidateIssuerSigningKey = true,
                        //ValidIssuer = jwtConfig.Issuer,
                        //ValidAudience = jwtConfig.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.SecretKey)),
                        ClockSkew = TimeSpan.Zero, // Elimina la tolerancia de tiempo
                    };

                    // Agrega eventos para registrar errores y éxitos
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<string>>();
                            logger.LogError(context.Exception, "Error en la autenticación.");
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<string>>();
                            logger.LogInformation("Token validado correctamente para el usuario {UserId}.", context.Principal?.Identity?.Name);
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<string>>();
                            logger.LogWarning("Se emitió un desafío de autenticación.");
                            return Task.CompletedTask;
                        }
                    };
                });

            services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };
            });

            services.Configure<JwtConfig>(config.GetSection("JwtConfig"));
        }
    }

}
