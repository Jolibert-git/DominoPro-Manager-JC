using Domino.Domain.Core;
using System.ComponentModel.DataAnnotations;
using static Domino.Domain.Entities.Enums;

namespace Domino.Domain.Entities
{
    public class Table: HasId
    {
        [Required]
        public int RoundId { get; set; }
       
        [Required]
        public int TableNumber { get; set; }

        public GameStatus Status { get; set; } = GameStatus.Pending;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        
        public bool WonBlock { get; set; } = false;
   
        public int? RemainingChips { get; set; }
     
        public Round Round { get; set; } = null!;


        public ICollection<Result> Results { get; set; } = new List<Result>();

        public bool IsComplete => Status == GameStatus.Completed;

        public double? Duration => StartDate.HasValue && EndDate.HasValue? (EndDate.Value - StartDate.Value).TotalMinutes: null;

        public Result? Winner => Results.FirstOrDefault(r => r.IsWinner);
        
    
    }
}
