using ReservaSalas.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace ReservaSalas.Api.Dtos.Requests;

/// <summary>
/// Datos utilizados para actualizar una reserva existente.
/// </summary>
public sealed class ActualizarReservaRequest : IValidatableObject
{
    /// <summary>
    /// Nombre de la sala reservada.
    /// </summary>
    [Required(ErrorMessage = "El nombre de la sala es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre de la sala no puede superar los 100 caracteres.")]
    public required string NombreSala { get; set; }

    /// <summary>
    /// Fecha de la reserva.
    /// </summary>
    public required DateOnly FechaReserva { get; set; }

    /// <summary>
    /// Hora de inicio de la reunión.
    /// </summary>
    public required TimeOnly HoraInicio { get; set; }

    /// <summary>
    /// Hora de finalización.
    /// Debe ser posterior a la hora de inicio.
    /// </summary>
    public required TimeOnly HoraFin { get; set; }

    /// <summary>
    /// Nombre de la persona responsable de la reserva.
    /// </summary>
    [Required(ErrorMessage = "El nombre del responsable es obligatorio.")]
    [MaxLength(100, ErrorMessage = "El nombre del responsable no puede superar los 100 caracteres.")]
    public required string NombreResponsable { get; set; }

    /// <summary>
    /// Cantidad de asistentes. Debe ser mayor a cero.
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad de asistentes debe ser mayor a cero.")]
    public required int CantidadAsistentes { get; set; }

    /// <summary>
    /// Motivo de la reunión.
    /// </summary>
    [Required(ErrorMessage = "El motivo de la reunión es obligatorio.")]
    [MaxLength(500, ErrorMessage = "El motivo de la reunión no puede superar los 500 caracteres.")]
    public required string Motivo { get; set; }

    /// <summary>
    /// Estado actual de la reserva.
    /// </summary>
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