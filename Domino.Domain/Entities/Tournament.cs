using Domino.Domain.Core;
using System.ComponentModel.DataAnnotations;
using static Domino.Domain.Entities.Enums;

namespace Domino.Domain.Entities
{
    public class Tournament: HasId
    {
        [Required(ErrorMessage = "You need insert Name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The name need stay between 2 to 40 caracter")]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TypeMode Mode { get; set; } = TypeMode.Doubles;
        [EnumDataType(typeof(TournamentStatus), ErrorMessage = "Tournament status not validated")]
        public TournamentStatus Status { get; set; } = TournamentStatus.Programmed;
        public string? Place { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        [Range(4, 1000, ErrorMessage = "Minimum players must be at least 4")]
        public short? MaxPlayers { get; set; }
        [Range(4, 1000, ErrorMessage = "Minimum players must be at least 4")]
        public short MinPlayers { get; set; } = 4;
        [Range(50, 5000, ErrorMessage = "Target points should be between 50 and 5000")]
        public int TargetPoint { get; set; } = 100;
        [Range(1, 50, ErrorMessage = "Rounds should be between 1 and 50")]
        public byte? MaxRound { get; set; }
        public string? Prize { get; set; } = string.Empty;
        
        public ICollection<Round> Rondas { get; set; } = new List<Round>();
        public ICollection<Classification> Classificationes { get; set; } = new List<Classification>();
        public ICollection<TournamentRegistration> Registrations { get; set; } = new List<TournamentRegistration>();
        public int TotalRegistered => Registrations.Count;
        public bool IsFull => MaxPlayers.HasValue && TotalRegistered >= MaxPlayers.Value;
    }
}
