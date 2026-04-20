using Domino.Domain.Core;
using System.ComponentModel.DataAnnotations;
using static Domino.Domain.Entities.Enums;

namespace Domino.Domain.Entities
{
    public class Tournament: HasId
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TypeMode Mode { get; set; } = TypeMode.Doubles;
        public TournamentStatus Status { get; set; } = TournamentStatus.Programmed;
        public string? Place { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
      
        public short? MaxPlayers { get; set; }
    
        public short MinPlayers { get; set; } = 4;
    
        public int TargetPoint { get; set; } = 100;
        public byte? MaxRound { get; set; }
        public string? Prize { get; set; } = string.Empty;
        
        public ICollection<Round> Rondas { get; set; } = new List<Round>();
        public ICollection<Classification> Classificationes { get; set; } = new List<Classification>();
        public ICollection<TournamentRegistration> Registrations { get; set; } = new List<TournamentRegistration>();
        public int TotalRegistered => Registrations.Count;
        public bool IsFull => MaxPlayers.HasValue && TotalRegistered >= MaxPlayers.Value;
    }
}
