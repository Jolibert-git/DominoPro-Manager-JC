using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.DTOs.PlayerDTOs
{
    public class PlayerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public short Elo { get; set; }
        public short WinGame { get; set; }
        public short LostGame { get; set; }
        public double WinRate { get; set; }
        public bool IsActive { get; set; }
    }
}
