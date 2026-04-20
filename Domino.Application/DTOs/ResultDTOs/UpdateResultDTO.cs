

using System.ComponentModel.DataAnnotations;

namespace Domino.Application.DTOs.ResultDTOs
{
    public class UpdateResultDTO
    {
        public int? Point { get; set; }

        public int? PointsAccumulated { get; set; }

        public bool? IsWinner { get; set; }

        public bool? WonBlock { get; set; }
        [Range(1, 270, ErrorMessage = "Out of range")]
        public byte? RemainingChips { get; set; }

        
        public byte? Position { get; set; }
    }
}
