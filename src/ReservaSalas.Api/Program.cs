var builder = WebApplication.CreateBuilder(args);

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