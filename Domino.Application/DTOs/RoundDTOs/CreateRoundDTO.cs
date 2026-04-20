
using System.ComponentModel.DataAnnotations;

namespace Domino.Application.DTOs.RoundDTOs
{
    public class CreateRoundDTO
    {
        [Required(ErrorMessage = "You need insert tournament Id")]
        public int TournamentId { get; set; }


        [Range(1, 100, ErrorMessage = "Round number must be between 1 and 100 ")]
        public byte RoundNumber { get; set; }

        public DateTime? StartDate { get; set; }
    }
}
