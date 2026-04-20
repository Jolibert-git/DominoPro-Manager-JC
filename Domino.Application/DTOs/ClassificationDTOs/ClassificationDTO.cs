using Domino.Application.DTOs.PlayerDTOs;
using System.ComponentModel.DataAnnotations;

namespace Domino.Application.DTOs.ClassificationDTOs
{
    public class ClassificationDTO
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public int PlayerId { get; set; }
        public int Position { get; set; }
        public byte TotalPercentage { get; set; }
        public short WinGame { get; set; }
        public short LostGame { get; set; }
        public int TotalGame { get; set; }
        public double WinRate { get; set; }
        public short FavorPoints { get; set; }
        public short PointsAgainst { get; set; }
        public int PointsDifferent { get; set; }
        public byte LastRoundCalculated { get; set; }
        public bool IsFinal { get; set; }
        public PlayerSummaryDTO? Player { get; set; }
    }
}
