using Domino.Domain.Core;
using System.ComponentModel.DataAnnotations;
using static Domino.Domain.Entities.Enums;

namespace Domino.Domain.Entities
{
    public class TournamentRegistration: HasId
    {
        [Required]
        public int PlayerId { get; set; }
        [Required]
        public int TournamentId { get; set; }

        
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;

        public short? DorsalNumber{ get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public bool PaymentFee { get; set; } = false;

        public Player Player { get; set; } = null!;

        public Tournament Tournament { get; set; } = null!;
    }
}
