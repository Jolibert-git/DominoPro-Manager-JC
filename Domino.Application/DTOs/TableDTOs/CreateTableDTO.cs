using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.DTOs.TableDTOs
{
    public class CreateTableDTO
    {
        [Required(ErrorMessage = "Round ID is required")]
        public int RoundId { get; set; }

        [Required]
        [Range(1, 200)]
        public int TableNumber { get; set; }
    }
}
