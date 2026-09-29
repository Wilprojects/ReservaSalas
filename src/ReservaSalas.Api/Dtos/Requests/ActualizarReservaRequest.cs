using ReservaSalas.Api.Models;

namespace ReservaSalas.Api.Dtos.Requests
{
    public sealed class ActualizarReservaRequest
    {
        public required string NombreSala { get; set; }

        public required DateOnly FechaReserva { get; set; }

        public required TimeOnly HoraInicio { get; set; }

        public required TimeOnly HoraFin { get; set; }

        public required string NombreResponsable { get; set; }

        public required int CantidadAsistentes { get; set; }

        public required string Motivo { get; set; }

        public required EstadoReserva Estado { get; set; }
    }
}
