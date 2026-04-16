using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.DTOs.TournamentDTOs
{
    public class CreateTournamentDTO
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [EnumDataType(typeof(TypeMode))]
        public TypeMode Mode { get; set; } = TypeMode.Doubles;

        public string? Place { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Range(4, 1000)]
        public short? MaxPlayers { get; set; }

        [Range(4, 1000)]
        public short MinPlayers { get; set; } = 4;

        [Range(50, 5000)]
        public int TargetPoint { get; set; } = 100;

        [Range(1, 50)]
        public byte? MaxRound { get; set; }

        public string? Prize { get; set; }
    }
}
