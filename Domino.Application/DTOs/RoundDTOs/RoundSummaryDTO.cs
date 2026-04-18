

namespace Domino.Application.DTOs.RoundDTOs
{
    public class RoundSummaryDTO
    {
        public int Id { get; set; }
        public byte RoundNumber { get; set; }
        public string Status { get; set; } = string.Empty;
        public int TotalTable { get; set; }
    }
}
