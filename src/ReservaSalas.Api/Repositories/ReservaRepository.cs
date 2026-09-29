using ReservaSalas.Api.Models;

namespace ReservaSalas.Api.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly List<Reserva> _reservas;

        private readonly object _lock = new();

        private int _nextId;

        public ReservaRepository()
        {
            _reservas = new List<Reserva>
            {
                new Reserva
                {
                    Id = 1,
                    NombreSala = "Sala Lima",
                    FechaReserva = new DateOnly(2026, 10, 15),
                    HoraInicio = new TimeOnly(9, 0),
                    HoraFin = new TimeOnly(10, 30),
                    NombreResponsable = "Ana Torres",
                    CantidadAsistentes = 8,
                    Motivo = "Reunión de planificación",
                    Estado = EstadoReserva.Pendiente
                },

                new Reserva
                {
                    Id = 2,
                    NombreSala = "Sala Cusco",
                    FechaReserva = new DateOnly(2026, 10, 15),
                    HoraInicio = new TimeOnly(11, 0),
                    HoraFin = new TimeOnly(12, 0),
                    NombreResponsable = "Carlos Mendoza",
                    CantidadAsistentes = 5,
                    Motivo = "Seguimiento del proyecto",
                    Estado = EstadoReserva.Confirmada
                },

                new Reserva
                {
                    Id = 3,
                    NombreSala = "Sala Arequipa",
                    FechaReserva = new DateOnly(2026, 10, 16),
                    HoraInicio = new TimeOnly(15, 0),
                    HoraFin = new TimeOnly(16, 30),
                    NombreResponsable = "Lucía Ramírez",
                    CantidadAsistentes = 12,
                    Motivo = "Presentación de resultados",
                    Estado = EstadoReserva.Cancelada
                }
            };

            _nextId = 4;
        }

        public IReadOnlyCollection<Reserva> GetAll()
        {
            lock (_lock)
            {
                return _reservas
                    .OrderBy(reserva => reserva.FechaReserva)
                    .ThenBy(reserva => reserva.HoraInicio)
                    .ToList();
            }
        }

        public IReadOnlyCollection<Reserva> GetByFecha(DateOnly fecha)
        {
            lock (_lock)
            {
                return _reservas
                    .Where(reserva => reserva.FechaReserva == fecha)
                    .OrderBy(reserva => reserva.HoraInicio)
                    .ToList();
            }
        }

        public Reserva? GetById(int id)
        {
            lock (_lock)
            {
                return _reservas.FirstOrDefault(reserva => reserva.Id == id);
            }
        }

        public Reserva Add(Reserva reserva)
        {
            lock (_lock)
            {
                reserva.Id = _nextId;

                _nextId++;

                _reservas.Add(reserva);

                return reserva;
            }
        }

        public bool Update(Reserva reserva)
        {
            lock (_lock)
            {
                var index = _reservas.FindIndex(item => item.Id == reserva.Id);

                if (index == -1)
                {
                    return false;
                }

                _reservas[index] = reserva;

                return true;
            }
        }

        public bool Delete(int id)
        {
            lock (_lock)
            {
                var reserva = _reservas.FirstOrDefault(item => item.Id == id);

                if (reserva is null)
                {
                    return false;
                }

                _reservas.Remove(reserva);

                return true;
            }
        }

    }
}
