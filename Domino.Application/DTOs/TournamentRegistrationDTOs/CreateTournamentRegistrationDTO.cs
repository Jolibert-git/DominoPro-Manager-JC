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
        [Required(ErrorMessage = "Player ID is required")]
        public int PlayerId { get; set; }

        [Required(ErrorMessage = "Tournament ID is required")]
        public int TournamentId { get; set; }

        [Range(1, 1000)]
        public short? DorsalNumber { get; set; }

        public bool PaymentFee { get; set; } = false;
    }
}
