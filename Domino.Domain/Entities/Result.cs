using Domino.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domino.Domain.Entities.Table;

namespace Domino.Domain.Entities
{
    public class Result: HasId
    {
        [Required(ErrorMessage = "You need insert Table Id")]
        public int TableId { get; set; }
        [Required(ErrorMessage = "You need insert Player Id")]
        public int PlayerId { get; set; }
        public int? PlaymateId { get; set; } 
        public int Point { get; set; } = 0;
        public int PointsAccumulated { get; set; } = 0;

        public bool IsWinner { get; set; } = false;
        public bool WonBlock { get; set; } = false;
        public byte? RemainingChips { get; set; }
        
        public int Elo { get; set; }

  
        [Required(ErrorMessage = "You need insert Position")]
        public byte Position { get; set; } = 2;

        public Table Table { get; set; } = null!;

        public Player Player { get; set; } = null!;


        public Player? Playmate { get; set; }
    }
}
