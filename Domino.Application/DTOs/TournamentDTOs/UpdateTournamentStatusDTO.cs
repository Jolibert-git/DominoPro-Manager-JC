
using System.ComponentModel.DataAnnotations;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.DTOs.TournamentDTOs
{
    public class UpdateTournamentStatusDTO
    {
        [Required]
        [EnumDataType(typeof(TournamentStatus), ErrorMessage = "Tournament status not validated")]
        public TournamentStatus Status { get; set; } = TournamentStatus.Programmed;
    }
}
