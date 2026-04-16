using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.DTOs.ResultDTOs
{
    public class CreateResultDTO
    {
        [Required(ErrorMessage = "Table ID is required")]
        public int TableId { get; set; }

        [Required(ErrorMessage = "Player ID is required")]
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
