

namespace Domino.Application.DTOs.TableDTOs
{
    public class TableDTO
    {
        public int Id { get; set; }
        public int RoundId { get; set; }
        public int TableNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public double? Duration { get; set; }
        public bool WonBlock { get; set; }
        public int? RemainingChips { get; set; }
        public bool IsComplete { get; set; }
    }
}
