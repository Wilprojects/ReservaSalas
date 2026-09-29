using ReservaSalas.Api.Models;

namespace ReservaSalas.Api.Dtos.Responses;

/// <summary>
/// Representa la información de una reserva devuelta por la API.
/// </summary>
public sealed class ReservaResponse
{
    /// <summary>
    /// Identificador único de la reserva.
    /// </summary>
    public required int Id { get; set; }

    /// <summary>
    /// Nombre de la sala reservada.
    /// </summary>
    public required string NombreSala { get; set; }

    /// <summary>
    /// Fecha de la reserva.
    /// </summary>
    public required DateOnly FechaReserva { get; set; }

    /// <summary>
    /// Hora de inicio de la reserva.
    /// </summary>
    public required TimeOnly HoraInicio { get; set; }

    /// <summary>
    /// Hora de finalización de la reserva.
    /// </summary>
    public required TimeOnly HoraFin { get; set; }

    /// <summary>
    /// Persona responsable de la reserva.
    /// </summary>
    public required string NombreResponsable { get; set; }

    /// <summary>
    /// Número de asistentes.
    /// </summary>
    public required int CantidadAsistentes { get; set; }

    /// <summary>
    /// Motivo de la reunión.
    /// </summary>
    public required string Motivo { get; set; }

    /// <summary>
    /// Estado actual de la reserva.
    /// </summary>
    public required EstadoReserva Estado { get; set; }
}