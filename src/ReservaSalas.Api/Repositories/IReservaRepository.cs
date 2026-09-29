using ReservaSalas.Api.Models;

namespace ReservaSalas.Api.Repositories;

public interface IReservaRepository
{
    IReadOnlyCollection<Reserva> GetAll();

    IReadOnlyCollection<Reserva> GetByFecha(DateOnly fecha);

    Reserva? GetById(int id);

    Reserva Add(Reserva reserva);

    bool Update(Reserva reserva);

    bool Delete(int id);
}