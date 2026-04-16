using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.DTOs.TournamentDTOs
{
    public class UpdateTournamentStatusDTO
    {
        [Required]
        [EnumDataType(typeof(TournamentStatus))]
        public TournamentStatus Status { get; set; }
    }
}
