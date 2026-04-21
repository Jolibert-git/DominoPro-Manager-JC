
using System.ComponentModel.DataAnnotations;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.DTOs.TournamentRegistrationDTOs
{
    public class UpdateTournamentRegistrationStatusDTO
    {
        [Required]
        [EnumDataType(typeof(RegistrationStatus), ErrorMessage = "Registration status no validated")]
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;

        public bool? PaymentFee { get; set; }
    }
}
