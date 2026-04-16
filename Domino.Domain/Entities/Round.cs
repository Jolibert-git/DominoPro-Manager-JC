using Domino.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domino.Domain.Entities.Enums;
using static Domino.Domain.Entities.Table;

namespace Domino.Domain.Entities
{
    public class Round: HasId
    {
        [Required(ErrorMessage = "You need insert tournament Id")]
        public int TournamentId { get; set ; }

       
        [Range(1, 100, ErrorMessage = "Round number must be between 1 and 100 ")]
        public byte RoundNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate{ get; set; }
        public ICollection<Table> Tables { get; set; } = new List<Table>();
        

        [EnumDataType(typeof(RoundStatus), ErrorMessage = "Status not validated")]
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
