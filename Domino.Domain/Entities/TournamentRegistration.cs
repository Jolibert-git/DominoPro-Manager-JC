using Domino.Domain.Core;
using System.ComponentModel.DataAnnotations;
using static Domino.Domain.Entities.Enums;

namespace Domino.Domain.Entities
{
    public class TournamentRegistration: HasId
    {
        [Required(ErrorMessage = "You need insert Player Id")]
        public int PlayerId { get; set; }
        [Required(ErrorMessage = "You need insert tournament Id")]
        public int TournamentId { get; set; }

        [EnumDataType(typeof(RegistrationStatus), ErrorMessage = "Registration status no validated")]
        public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;

        [Range(1, 1000, ErrorMessage = "Dolsal number our of range")]
        public short? DorsalNumber{ get; set; }

        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;

        public bool PaymentFee { get; set; } = false;

        public Player Player { get; set; } = null!;

        public Tournament Tournament { get; set; } = null!;
    }
}
