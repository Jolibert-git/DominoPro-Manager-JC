using Domino.Application.DTOs.PlayerDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.DTOs.ResultDTOs
{
    public class ResultDTO
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int PlayerId { get; set; }
        public int? PlaymateId { get; set; }
        public int Point { get; set; }
        public int PointsAccumulated { get; set; }
        public bool IsWinner { get; set; }
        public bool WonBlock { get; set; }
        public byte? RemainingChips { get; set; }
        public int Elo { get; set; }
        public byte Position { get; set; }
        public PlayerSummaryDTO? Player { get; set; }
        public PlayerSummaryDTO? Playmate { get; set; }
    }
}
