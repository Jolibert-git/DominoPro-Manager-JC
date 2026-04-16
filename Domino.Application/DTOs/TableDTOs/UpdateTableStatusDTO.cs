using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.DTOs.TableDTOs
{
    public class UpdateTableStatusDTO
    {
        [Required]
        [EnumDataType(typeof(GameStatus))]
        public GameStatus Status { get; set; }

        public DateTime? EndDate { get; set; }

        public bool? WonBlock { get; set; }

        [Range(0, 500)]
        public int? RemainingChips { get; set; }
    }
}
