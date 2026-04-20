using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.DTOs.TournamentRegistrationDTOs
{
    public class CreateTournamentRegistrationDTO
    {
        [Required(ErrorMessage = "You need insert Player Id")]
        public int PlayerId { get; set; }

        [Required(ErrorMessage = "You need insert tournament Id")]
        public int TournamentId { get; set; }

        [Range(1, 1000, ErrorMessage = "Dolsal number our of range")]
        public short? DorsalNumber { get; set; }

        public bool PaymentFee { get; set; } = false;
    }
}
