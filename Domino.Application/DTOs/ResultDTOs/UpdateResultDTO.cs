using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.DTOs.ResultDTOs
{
    public class UpdateResultDTO
    {
        public int? Point { get; set; }

        public int? PointsAccumulated { get; set; }

        public bool? IsWinner { get; set; }

        public bool? WonBlock { get; set; }

        public byte? RemainingChips { get; set; }

        public int? Elo { get; set; }

        public byte? Position { get; set; }
    }
}
