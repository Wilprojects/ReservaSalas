using Microsoft.AspNetCore.Http.HttpResults;
using ReservaSalas.Api.Dtos.Requests;
using ReservaSalas.Api.Dtos.Responses;
using ReservaSalas.Api.Mappings;
using ReservaSalas.Api.Repositories;

namespace ReservaSalas.Api.Endpoints;

public static class ReservasEndpoints
{
    public static IEndpointRouteBuilder MapReservasEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/reservas")
            .WithTags("Reservas");

        group.MapGet("", GetReservas)
            .WithName("GetReservas")
            .Produces(StatusCodes.Status400BadRequest);

        group.MapGet("/{id:int}", GetReservaById)
            .WithName("GetReservaById");

        group.MapPost("", CreateReserva)
            .WithName("CreateReserva")
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapPut("/{id:int}", UpdateReserva)
            .WithName("UpdateReserva")
            .ProducesValidationProblem(StatusCodes.Status400BadRequest);

        group.MapDelete("/{id:int}", DeleteReserva)
            .WithName("DeleteReserva");

        return endpoints;
    }


    /// <summary>
    /// Obtiene las reservas registradas.
    /// </summary>
    /// <remarks>
    /// Si se proporciona el parámetro fecha, devuelve únicamente
    /// las reservas correspondientes a dicha fecha.
    /// Si no se proporciona, devuelve todas las reservas.
    /// </remarks>
    /// <param name="fecha" example="2026-10-15">
    /// Fecha opcional utilizada para filtrar las reservas.
    /// </param>
    /// <response code="200">
    /// Reservas obtenidas correctamente.
    /// </response>
    /// <response code="400">
    /// El formato del parámetro fecha no es válido.
    /// </response>
    public static Ok<List<ReservaResponse>> GetReservas(DateOnly? fecha, IReservaRepository repository)
    {
        var reservas = fecha.HasValue
            ? repository.GetByFecha(fecha.Value)
            : repository.GetAll();

        var response = reservas
            .Select(reserva => reserva.ToResponse())
            .ToList();

        return TypedResults.Ok(response);
    }


    /// <summary>
    /// Obtiene una reserva por su identificador.
    /// </summary>
    /// <param name="id" example="1">
    /// Identificador único de la reserva.
    /// </param>
    /// <response code="200">
    /// Reserva encontrada correctamente.
    /// </response>
    /// <response code="404">
    /// No existe una reserva con el identificador indicado.
    /// </response>
    public static Results<Ok<ReservaResponse>, NotFound> GetReservaById(int id, IReservaRepository repository)
    {
        var reserva = repository.GetById(id);

        if (reserva is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(reserva.ToResponse());
    }


    /// <summary>
    /// Registra una nueva reserva de sala.
    /// </summary>
    /// <remarks>
    /// Toda nueva reserva se registra inicialmente con estado Pendiente.
    /// El identificador es generado automáticamente por la API.
    /// </remarks>
    /// <param name="request">
    /// Datos necesarios para registrar la reserva.
    /// </param>
    /// <response code="201">
    /// Reserva creada correctamente.
    /// </response>
    /// <response code="400">
    /// Los datos enviados no cumplen las reglas de validación.
    /// </response>
    public static Created<ReservaResponse> CreateReserva(CrearReservaRequest request, IReservaRepository repository)
    {
        var reserva = request.ToModel();

        var reservaCreada = repository.Add(reserva);

        var response = reservaCreada.ToResponse();

        return TypedResults.Created($"/api/reservas/{response.Id}", response);
    }


    /// <summary>
    /// Actualiza una reserva existente.
    /// </summary>
    /// <param name="id" example="1">
    /// Identificador de la reserva que será actualizada.
    /// </param>
    /// <param name="request">
    /// Nuevos datos de la reserva.
    /// </param>
    /// <response code="200">
    /// Reserva actualizada correctamente.
    /// </response>
    /// <response code="400">
    /// Los datos enviados no cumplen las reglas de validación.
    /// </response>
    /// <response code="404">
    /// No existe una reserva con el identificador indicado.
    /// </response>
    public static Results<Ok<ReservaResponse>, NotFound> UpdateReserva(int id, ActualizarReservaRequest request, IReservaRepository repository)
    {
        var reservaExistente = repository.GetById(id);

        if (reservaExistente is null)
        {
            return TypedResults.NotFound();
        }

        var reservaActualizada = request.ToModel(id);

        repository.Update(reservaActualizada);

        return TypedResults.Ok(reservaActualizada.ToResponse());
    }


    /// <summary>
    /// Elimina una reserva existente.
    /// </summary>
    /// <param name="id" example="1">
    /// Identificador de la reserva que será eliminada.
    /// </param>
    /// <response code="204">
    /// Reserva eliminada correctamente.
    /// </response>
    /// <response code="404">
    /// No existe una reserva con el identificador indicado.
    /// </response>
    public static Results<NoContent, NotFound> DeleteReserva(int id, IReservaRepository repository)
    {
        var eliminada = repository.Delete(id);

        if (!eliminada)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.NoContent();
    }

}