using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
