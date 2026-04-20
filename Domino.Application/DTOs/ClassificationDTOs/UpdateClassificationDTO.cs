
using System.ComponentModel.DataAnnotations;

namespace Domino.Application.DTOs.ClassificationDTOs
{
    public class UpdateClassificationDTO
    {
        [Required(ErrorMessage = "You need insert Tournament Id")]
        public int TournamentId { get; set; }

        [Required(ErrorMessage = "You need insert Player Id")]
        public int PlayerId { get; set; }

        public int Position { get; set; }

        [Range(0, 100, ErrorMessage = "The percentage must be kept between 0 and 100")]
        public byte TotalPercentage { get; set; } = 0;

        [Range(0, 32000, ErrorMessage = "Out of range")]
        public short WinGame { get; set; } = 0;

        [Range(0, 32000, ErrorMessage = "Out of range")]
        public short LostGame { get; set; } = 0;

        [Range(0, 32000, ErrorMessage = "Out of range")]
        public short FavorPoints { get; set; } = 0;

        [Range(0, 32000, ErrorMessage = "Out of range")]
        public short PointsAgainst { get; set; } = 0;

        [Range(0, 100, ErrorMessage = "Out of range")]
        public byte LastRoundCalculated { get; set; } = 0;

        public bool IsFinal { get; set; } = false;
    }
}
