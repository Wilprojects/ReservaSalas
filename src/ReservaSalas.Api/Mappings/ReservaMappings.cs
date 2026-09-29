using ReservaSalas.Api.Dtos.Requests;
using ReservaSalas.Api.Dtos.Responses;
using ReservaSalas.Api.Models;

namespace ReservaSalas.Api.Mappings;

public static class ReservaMappings
{
    public static Reserva ToModel(this CrearReservaRequest request)
    {
        return new Reserva
        {
            NombreSala = request.NombreSala,
            FechaReserva = request.FechaReserva,
            HoraInicio = request.HoraInicio,
            HoraFin = request.HoraFin,
            NombreResponsable = request.NombreResponsable,
            CantidadAsistentes = request.CantidadAsistentes,
            Motivo = request.Motivo,
            Estado = EstadoReserva.Pendiente
        };
    }

    public static Reserva ToModel(this ActualizarReservaRequest request, int id)
    {
        return new Reserva
        {
            Id = id,
            NombreSala = request.NombreSala,
            FechaReserva = request.FechaReserva,
            HoraInicio = request.HoraInicio,
            HoraFin = request.HoraFin,
            NombreResponsable = request.NombreResponsable,
            CantidadAsistentes = request.CantidadAsistentes,
            Motivo = request.Motivo,
            Estado = request.Estado
        };
    }

    public static ReservaResponse ToResponse(this Reserva reserva)
    {
        return new ReservaResponse
        {
            Id = reserva.Id,
            NombreSala = reserva.NombreSala,
            FechaReserva = reserva.FechaReserva,
            HoraInicio = reserva.HoraInicio,
            HoraFin = reserva.HoraFin,
            NombreResponsable = reserva.NombreResponsable,
            CantidadAsistentes = reserva.CantidadAsistentes,
            Motivo = reserva.Motivo,
            Estado = reserva.Estado
        };
    }
}