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
            .WithName("GetReservas");

        group.MapGet("/{id:int}", GetReservaById)
            .WithName("GetReservaById");

        group.MapPost("", CreateReserva)
            .WithName("CreateReserva");

        group.MapPut("/{id:int}", UpdateReserva)
            .WithName("UpdateReserva");

        group.MapDelete("/{id:int}", DeleteReserva)
            .WithName("DeleteReserva");

        return endpoints;
    }

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

    public static Results<Ok<ReservaResponse>, NotFound> GetReservaById(int id, IReservaRepository repository)
    {
        var reserva = repository.GetById(id);

        if (reserva is null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(reserva.ToResponse());
    }

    public static Created<ReservaResponse> CreateReserva(CrearReservaRequest request, IReservaRepository repository)
    {
        var reserva = request.ToModel();

        var reservaCreada = repository.Add(reserva);

        var response = reservaCreada.ToResponse();

        return TypedResults.Created($"/api/reservas/{response.Id}", response);
    }

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