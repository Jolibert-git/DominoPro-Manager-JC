
using System.ComponentModel.DataAnnotations;

namespace Domino.Application.DTOs.PlayerDTOs
{
    public class UpdatePlayerDTO
    {
        [Required(ErrorMessage = "You need insert the name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The name need stay between 2 to 40 caracter")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "You need insert the last name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "The last name need stay between 2 to 40 caracter")]
        public string? LastName { get; set; }

        [EmailAddress]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The Email need stay between 5 to 100 caracter")]
        public string? Email { get; set; }
        [StringLength(15)]
        [Phone]
        public string? Phone { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
