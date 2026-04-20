
using System.ComponentModel.DataAnnotations;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.DTOs.TableDTOs
{
    public class UpdateTableStatusDTO
    {
        [Required]
        [EnumDataType(typeof(GameStatus), ErrorMessage = "Sattus not validated")]
        public GameStatus Status { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? WonBlock { get; set; }

        [Range(0, 500, ErrorMessage = "Remaining chips sum is invalid")]
        public int? RemainingChips { get; set; }
    }
}
