
using System.ComponentModel.DataAnnotations;
namespace Domino.Application.DTOs.TableDTOs
{
    public class CreateTableDTO
    {
        [Required(ErrorMessage = "You need insert round Id")]
        public int RoundId { get; set; }

        [Required(ErrorMessage = "You need insert table number")]
        [Range(1, 200, ErrorMessage = "Our of Range")]
        public short TableNumber { get; set; }
    }
}
