
using System.ComponentModel.DataAnnotations;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.DTOs.TournamentDTOs
{
    public class CreateTournamentDTO
    {
        [Required(ErrorMessage = "You need insert Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The name need stay between 2 to 40 caracter")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [EnumDataType(typeof(TypeMode), ErrorMessage = "Tournament status not validated")]
        public TypeMode Mode { get; set; } = TypeMode.Doubles;

        public string? Place { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Range(4, 1000, ErrorMessage = "Minimum players must be at least 4")]
        public short? MaxPlayers { get; set; }

        [Range(4, 1000, ErrorMessage = "Maximum players must be at least 4")]
        public short? MinPlayers { get; set; } = 4;

        [Range(50, 5000, ErrorMessage = "Target points should be between 50 and 5000")]
        public int TargetPoint { get; set; } = 100;

        [Range(1, 50, ErrorMessage = "Rounds should be between 1 and 50")]
        public byte? MaxRound { get; set; }

        public string? Prize { get; set; }
    }
}
