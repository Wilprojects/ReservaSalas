using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
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

// Autenticación JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

builder.Services.AddAuthorization();

// Servicios de OpenAPI
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;

    options.AddDocumentTransformer(
        (document, context, cancellationToken) =>
        {
            // Información general de la API
            document.Info = new()
            {
                Title = "API de Gestión de Reservas de Salas",
                Version = "1.0.0",
                Description = "API REST desarrollada en ASP.NET Core para administrar reservas de salas de reuniones de una organización."
            };

            // Definición del esquema Bearer JWT
            document.Components ??= new OpenApiComponents();

            document.Components.SecuritySchemes =
                new Dictionary<string, IOpenApiSecurityScheme>
                {
                    ["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "Ingrese el token JWT para autenticar las solicitudes."
                    }
                };

            // Aplicar Bearer solamente a /api/reservas
            foreach (var path in document.Paths)
            {
                if (!path.Key.StartsWith("/api/reservas", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                var operations = path.Value.Operations;

                if (operations is null)
                {
                    continue;
                }

                foreach (var operation in operations.Values)
                {
                    operation.Security ??= [];

                    operation.Security.Add(
                        new OpenApiSecurityRequirement
                        {
                            [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                        });
                }
            }

            return Task.CompletedTask;
        });
});


var app = builder.Build();

// Documento OpenAPI disponible solo en desarrollo
if (app.Environment.IsDevelopment())
{
    // OpenAPI en JSON
    app.MapOpenApi();

    // OpenAPI en YAML
    app.MapOpenApi("/openapi/{documentName}.yaml");

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API de Gestión de Reservas de Salas v1");

        options.DocumentTitle = "API de Gestión de Reservas de Salas";

        options.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () =>
{
    return Results.Ok(new
    {
        message = "API de Gestión de Reservas de Salas",
        status = "running"
    });
})
.AllowAnonymous();

app.MapReservasEndpoints();

app.Run();