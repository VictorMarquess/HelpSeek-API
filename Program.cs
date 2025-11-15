using Microsoft.EntityFrameworkCore;
using HelpSeek.API.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS (libera tudo por enquanto; ajuste para produção)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// Configura DbContext (banco HelpSeek)
builder.Services.AddDbContext<HelpSeekContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// ✅ habilita servir HTML, CSS e JS da pasta wwwroot
app.UseStaticFiles();

app.MapControllers();
app.Run();
