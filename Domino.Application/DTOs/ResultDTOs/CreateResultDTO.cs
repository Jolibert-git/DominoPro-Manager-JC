
using System.ComponentModel.DataAnnotations;

namespace Domino.Application.DTOs.ResultDTOs
{
    public class CreateResultDTO
    {
        [Required(ErrorMessage = "You need insert Table Id")]
        public int TableId { get; set; }

        [Required(ErrorMessage = "You need insert Player Id")]
        public int PlayerId { get; set; }

        public int? PlaymateId { get; set; }

        public int Point { get; set; } = 0;

        public int PointsAccumulated { get; set; } = 0;

        public bool IsWinner { get; set; } = false;

        public bool WonBlock { get; set; } = false;

        public byte? RemainingChips { get; set; }

        public int Elo { get; set; } = 0;

        [Range(1, 2)]
        public byte Position { get; set; } = 2;
    }
}
