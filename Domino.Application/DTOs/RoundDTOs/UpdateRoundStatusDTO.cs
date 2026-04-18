
using System.ComponentModel.DataAnnotations;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.DTOs.RoundDTOs
{
    public class UpdateRoundStatusDTO
    {
        [Required]
        [EnumDataType(typeof(RoundStatus))]
        public RoundStatus Status { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
