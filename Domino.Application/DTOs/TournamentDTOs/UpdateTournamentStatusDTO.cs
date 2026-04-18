
using System.ComponentModel.DataAnnotations;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.DTOs.TournamentDTOs
{
    public class UpdateTournamentStatusDTO
    {
        [Required]
        [EnumDataType(typeof(TournamentStatus))]
        public TournamentStatus Status { get; set; }
    }
}
