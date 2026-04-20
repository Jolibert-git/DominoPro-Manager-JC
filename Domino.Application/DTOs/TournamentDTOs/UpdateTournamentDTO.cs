
using System.ComponentModel.DataAnnotations;

namespace Domino.Application.DTOs.TournamentDTOs
{
    public class UpdateTournamentDTO
    {
        [Required(ErrorMessage = "You need insert Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The name need stay between 2 to 40 caracter")]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Place { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Range(4, 1000, ErrorMessage = "Minimum players must be at least 4")]
        public short? MaxPlayers { get; set; }

        [Range(50, 5000, ErrorMessage = "Target points should be between 50 and 5000")]
        public short? TargetPoint { get; set; }

        [Range(1, 50, ErrorMessage = "Rounds should be between 1 and 50")]
        public byte? MaxRound { get; set; }

        public string? Prize { get; set; }
    }
}
