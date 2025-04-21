using BlogApp.Server.App.Services;
using BlogApp.Server.Config;
using BlogApp.Server.Domain.Mappers;
using BlogApp.Server.Infra;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerWithJwt();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString") ?? throw new InvalidOperationException("Connection string 'ConnectionString' not found.")));

//builder.Services.ConfigureAuth(builder.Configuration);

builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.ConfigureRepositories();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("*")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
