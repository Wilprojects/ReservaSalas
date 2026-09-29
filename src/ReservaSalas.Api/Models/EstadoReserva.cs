namespace ReservaSalas.Api.Models;

/// <summary>
/// Estados posibles de una reserva de sala.
/// </summary>
public enum EstadoReserva
{
    /// <summary>
    /// La reserva fue registrada pero aún no ha sido confirmada.
    /// </summary>
    Pendiente,

    /// <summary>
    /// La reserva fue confirmada.
    /// </summary>
    Confirmada,

    /// <summary>
    /// La reserva fue cancelada.
    /// </summary>
    Cancelada
}