using Domino.Domain.Core;
using System.ComponentModel.DataAnnotations;
using static Domino.Domain.Entities.Enums;

namespace Domino.Domain.Entities
{
    public class Round: HasId
    {
        [Required]
        public int TournamentId { get; set ; }

       
        public byte RoundNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate{ get; set; }
        public ICollection<Table> Tables { get; set; } = new List<Table>();
        

       
        public RoundStatus Status { get; set; } = RoundStatus.Pending; 

        public Tournament Tournament { get; set; } = null!;

        public int TotalTable => Tables.Count;

        public int CompleteTable => Tables.Count(m => m.IsComplete);

        public double? Duration =>
        StartDate.HasValue && EndDate.HasValue
            ? (EndDate.Value - StartDate.Value ).TotalMinutes
            : null;
    }
}
