using Domino.Domain.Core;
using System.ComponentModel.DataAnnotations;

namespace Domino.Domain.Entities
{
    public class Result: HasId
    {
        [Required]
        public int TableId { get; set; }
        [Required]
        public int PlayerId { get; set; }
        public int? PlaymateId { get; set; } 
        public int Point { get; set; } = 0;
        public int PointsAccumulated { get; set; } = 0;

        public bool IsWinner { get; set; } = false;
        public bool WonBlock { get; set; } = false;
        public byte? RemainingChips { get; set; }
        
        public int Elo { get; set; }

  
        [Required]
        public byte Position { get; set; } = 2;

        public Table Table { get; set; } = null!;

        public Player Player { get; set; } = null!;


        public Player? Playmate { get; set; }
    }
}
