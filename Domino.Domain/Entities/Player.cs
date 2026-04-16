using Domino.Domain.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Domain.Entities
{
    public class Player: HasId
    {
        [Required(ErrorMessage = "You need insert the name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The name need stay between 2 to 40 caracter")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "You need insert the last name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The last name need stay between 2 to 40 caracter")]
        public string? LastName { get; set; }
        [StringLength(50, MinimumLength = 5, ErrorMessage = "The Email need stay between 2 to 40 caracter")]
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        [Range(0, 3000, ErrorMessage = "Out of range")]
        public short Elo { get; set; } = 0;
        [Range(0, 32000, ErrorMessage = "Out of range")]
        public int WinGame { get; set; }
        [Range(0, 32000, ErrorMessage = "Out of range")]
        public int LostGame { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<TournamentRegistration> Registration { get; set; } = new List<TournamentRegistration>();
        public ICollection<Result> Results { get; set; } = new List<Result>();
        public ICollection<Classification> Classifications { get; set; } = new List<Classification>();
        public double WinRate
        {
            get
            {
                int total = WinGame + LostGame;
                return total == 0 ? 0 : Math.Round((double)WinGame / total * 100, 2);
            }
        }

    }
}
