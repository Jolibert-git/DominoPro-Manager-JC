using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.DTOs.RoundDTOs
{
    public class UpdateRoundStatusDTO
    {
        [Required]
        [EnumDataType(typeof(RoundStatus))]
        public RoundStatus Status { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
