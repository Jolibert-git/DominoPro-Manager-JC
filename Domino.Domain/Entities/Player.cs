using Domino.Domain.Core;
using System.ComponentModel.DataAnnotations;

namespace Domino.Domain.Entities
{
    public class Player: HasId
    {
        [Required]
        [StringLength(50)]
        public string? Name { get; set; }
        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;
        [StringLength(15)]
        [Phone]
        public string Phone { get; set; } = string.Empty;
 
        
        public short Elo { get; set; } = 0;
        public int WinGame { get; set; }
     
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

                return total ==0? 0 : Math.Round((double)WinGame/ total * 100, 2);
            }
        }

    }
}
