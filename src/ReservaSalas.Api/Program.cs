using ReservaSalas.Api.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter<EstadoReserva>(allowIntegerValues: false));
});

// Validación automática de Minimal APIs
builder.Services.AddValidation();

// Servicios de OpenAPI
builder.Services.AddOpenApi();

var app = builder.Build();

// Documento OpenAPI disponible solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        message = "API de Gestión de Reservas de Salas",
        status = "running"
    });
});

app.Run();