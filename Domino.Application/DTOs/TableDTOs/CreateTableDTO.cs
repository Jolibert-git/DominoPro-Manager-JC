
using System.ComponentModel.DataAnnotations;
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
