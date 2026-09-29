namespace ReservaSalas.Api.Models
{
    public sealed class Reserva
    {
        public int Id { get; set; }

        public required string NombreSala { get; set; }

        public required DateOnly FechaReserva { get; set; }

        public required TimeOnly HoraInicio { get; set; }

        public required TimeOnly HoraFin { get; set; }

        public required string NombreResponsable { get; set; }

        public required int CantidadAsistentes { get; set; }

        public required string Motivo { get; set; }

        public EstadoReserva Estado { get; set; } = EstadoReserva.Pendiente;
    }
}
