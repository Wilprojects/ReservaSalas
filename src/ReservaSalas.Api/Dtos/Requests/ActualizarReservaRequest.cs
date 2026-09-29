using ReservaSalas.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace ReservaSalas.Api.Dtos.Requests;

public sealed class ActualizarReservaRequest : IValidatableObject
{
    [Required(ErrorMessage = "El nombre de la sala es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre de la sala no puede superar los 100 caracteres.")]
    public required string NombreSala { get; set; }

    public required DateOnly FechaReserva { get; set; }

    public required TimeOnly HoraInicio { get; set; }

    public required TimeOnly HoraFin { get; set; }

    [Required(ErrorMessage = "El nombre del responsable es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre del responsable no puede superar los 100 caracteres.")]
    public required string NombreResponsable { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad de asistentes debe ser mayor a cero.")]
    public required int CantidadAsistentes { get; set; }

    [Required(ErrorMessage = "El motivo de la reunión es obligatorio.")]
    [StringLength(500, ErrorMessage = "El motivo de la reunión no puede superar los 500 caracteres.")]
    public required string Motivo { get; set; }

    public required EstadoReserva Estado { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (HoraFin <= HoraInicio)
        {
            yield return new ValidationResult(
                "La hora de fin debe ser posterior a la hora de inicio.",
                new[]
                {
                    nameof(HoraFin)
                });
        }
    }
}