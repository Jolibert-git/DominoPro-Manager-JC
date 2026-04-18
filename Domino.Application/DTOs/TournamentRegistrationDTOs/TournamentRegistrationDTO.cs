using Domino.Application.DTOs.PlayerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.DTOs.TournamentRegistrationDTOs
{
    public class TournamentRegistrationDTO
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public int TournamentId { get; set; }
        public string Status { get; set; } = string.Empty;
        public short? DorsalNumber { get; set; }
        public bool PaymentFee { get; set; }
        public DateTime RegistrationDate { get; set; }
        public PlayerSummaryDTO? Player { get; set; }
    }
}
