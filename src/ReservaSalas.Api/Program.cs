using ReservaSalas.Api.Endpoints;
using ReservaSalas.Api.Models;
using ReservaSalas.Api.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter<EstadoReserva>(allowIntegerValues: false));
});

// Validación automática de Minimal APIs
builder.Services.AddValidation();

// Repositorio en memoria
builder.Services.AddSingleton<IReservaRepository, ReservaRepository>();

// Servicios de OpenAPI
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;

    options.AddDocumentTransformer((document, context, cancellationToken) =>
        {
            document.Info = new()
            {
                Title = "API de Gestión de Reservas de Salas",
                Version = "1.0.0",
                Description = "API REST desarrollada en ASP.NET Core para administrar reservas de salas de reuniones de una organización."
            };

            return Task.CompletedTask;
        });
});

var app = builder.Build();

// Documento OpenAPI disponible solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API de Gestión de Reservas de Salas v1");
        options.DocumentTitle = "API de Gestión de Reservas de Salas";

        options.DisplayRequestDuration();
    });
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

app.MapReservasEndpoints();

app.Run();