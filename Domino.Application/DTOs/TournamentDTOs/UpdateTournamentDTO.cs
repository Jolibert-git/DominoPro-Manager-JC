
using System.ComponentModel.DataAnnotations;

namespace Domino.Application.DTOs.TournamentDTOs
{
    public class UpdateTournamentDTO
    {
        [StringLength(50, MinimumLength = 2)]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? Place { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Range(4, 1000)]
        public short? MaxPlayers { get; set; }

        [Range(50, 5000)]
        public int? TargetPoint { get; set; }

        [Range(1, 50)]
        public byte? MaxRound { get; set; }

        public string? Prize { get; set; }
    }
}
