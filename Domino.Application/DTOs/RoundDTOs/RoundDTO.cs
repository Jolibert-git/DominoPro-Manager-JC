using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.DTOs.RoundDTOs
{
    public class RoundDTO
    {
        public int Id { get; set; }
        public int TournamentId { get; set; }
        public byte RoundNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? Duration { get; set; }
        public int TotalTable { get; set; }
        public int CompleteTable { get; set; }
    }
}
