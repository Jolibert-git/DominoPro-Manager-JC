using Domino.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domino.Domain.Entities.Enums;

namespace Domino.Domain.Entities
{
    public class Table: HasId
    {
        [Required(ErrorMessage = "You need insert round Id")]
        public int RoundId { get; set; }
       
        [Required(ErrorMessage = "You need insert table number")]
        [Range(1, 200, ErrorMessage = "Our of Range")]
        public int TableNumber { get; set; }
        [EnumDataType(typeof(GameStatus), ErrorMessage = "Sattus not validated")]
        public GameStatus Status { get; set; } = GameStatus.Pending;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        
        public bool WonBlock { get; set; } = false;
        
        [Range(0, 500, ErrorMessage = "Remaining chips sum is invalid")]
        public int? RemainingChips { get; set; }
     
        public Round Round { get; set; } = null!;


        public ICollection<Result> Results { get; set; } = new List<Result>();

        public bool IsComplete => Status == GameStatus.Completed;

        public double? Duration =>
            StartDate.HasValue && EndDate.HasValue
                ? (EndDate.Value - StartDate.Value).TotalMinutes
                : null;
        public Result? Winner => Results.FirstOrDefault(r => r.IsWinner);
        
    
    }
}
