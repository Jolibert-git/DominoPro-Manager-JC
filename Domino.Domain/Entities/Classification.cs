using Domino.Domain.Core;
using System.ComponentModel.DataAnnotations;

namespace Domino.Domain.Entities
{
    public class Classification: HasId
    {
        [Required]
        public int TournamentId { get; set; }
        [Required]
        public int PlayerId { get; set; }
        public int Position { get; set; }
       
        public byte TotalPercentage { get; set; } = 0;
       
        public short WinGame { get; set; } = 0;
        
        public short LostGame { get; set; } = 0;
       
        public short FavorPoints { get; set; } = 0;
        
        public short PointsAgainst { get; set; } = 0;
        public int PointsDifferent => FavorPoints - PointsAgainst;
     
        public byte LastRoundCalculated { get; set; } = 0;
        public bool IsFinal { get; set; } = false;
        public Tournament Tournament { get; set; } = null!;

        public Player Player { get; set; } = null!;
        public int TotalGame => WinGame + LostGame;


        public double WinRate => TotalGame==0?0:Math.Round((double) WinGame / TotalGame * 100, 2);
    }
}
