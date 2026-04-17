
using System.ComponentModel.DataAnnotations;

namespace Domino.Application.DTOs.ClassificationDTOs
{
    public class UpdateClassificationDTO
    {
        [Required]
        public int TournamentId { get; set; }

        [Required]
        public int PlayerId { get; set; }

        public int Position { get; set; }

        [Range(0, 100)]
        public byte TotalPercentage { get; set; } = 0;

        [Range(0, 32000)]
        public short WinGame { get; set; } = 0;

        [Range(0, 32000)]
        public short LostGame { get; set; } = 0;

        [Range(0, 32000)]
        public short FavorPoints { get; set; } = 0;

        [Range(0, 32000)]
        public short PointsAgainst { get; set; } = 0;

        [Range(0, 255)]
        public byte LastRoundCalculated { get; set; } = 0;

        public bool IsFinal { get; set; } = false;
    }
}
