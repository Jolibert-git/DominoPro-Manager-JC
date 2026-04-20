using System.ComponentModel.DataAnnotations;

namespace Domino.Application.DTOs.PlayerDTOs
{
    public class CreatePlayerDTO
    {
        [Required(ErrorMessage = "You need insert the name")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "The name need stay between 2 to 40 caracter")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "You need insert the last name")]
        [StringLength(40, MinimumLength = 2, ErrorMessage = "The last name need stay between 2 to 40 caracter")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "The Email need stay between 8 to 100 caracter")]
        public string Email { get; set; } = string.Empty;
        [StringLength(15)]
        [Phone(ErrorMessage = "Invalid phone format")]

        public string Phone { get; set; } = string.Empty;

    }
}
