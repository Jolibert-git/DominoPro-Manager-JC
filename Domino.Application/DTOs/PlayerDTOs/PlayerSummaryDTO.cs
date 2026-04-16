using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.DTOs.PlayerDTOs
{
    public class PlayerSummaryDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public short Elo { get; set; }
        public bool IsActive { get; set; }
    }
}
