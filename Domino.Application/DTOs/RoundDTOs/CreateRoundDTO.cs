
using System.ComponentModel.DataAnnotations;

namespace Domino.Application.DTOs.RoundDTOs
{
    public class CreateRoundDTO
    {
        [Required(ErrorMessage = "Tournament ID is required")]
        public int TournamentId { get; set; }

        [Range(1, 100)]
        public byte RoundNumber { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
