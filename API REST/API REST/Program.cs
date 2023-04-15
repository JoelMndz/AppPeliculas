using API_REST.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Contexto>(options =>
    {
        options.UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll);
        options.UseSqlServer(builder.Configuration.GetConnectionString("Conexion"));
    },
    ServiceLifetime.Transient);

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "public")),
    RequestPath = "/public"
});

app.UseCors(builder => {
    builder.AllowAnyOrigin()
           .AllowAnyHeader()
           .AllowAnyMethod();
});


app.UseAuthorization();

app.MapControllers();

app.Run();
