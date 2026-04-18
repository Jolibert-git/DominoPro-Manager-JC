

namespace Domino.Application.DTOs.TournamentDTOs
{
    public class TournamentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Mode { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Place { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short? MaxPlayers { get; set; }
        public short MinPlayers { get; set; }
        public int TargetPoint { get; set; }
        public byte? MaxRound { get; set; }
        public string? Prize { get; set; }
        public int TotalRegistered { get; set; }
        public bool IsFull { get; set; }
        public DateTime CreatedIn { get; set; }
    }
}
