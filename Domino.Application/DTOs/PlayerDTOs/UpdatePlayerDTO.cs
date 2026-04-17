
using System.ComponentModel.DataAnnotations;

namespace Domino.Application.DTOs.PlayerDTOs
{
    public class UpdatePlayerDTO
    {
        [StringLength(50, MinimumLength = 2)]
        public string? Name { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string? LastName { get; set; }

        [StringLength(100, MinimumLength = 5)]
        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public bool? IsActive { get; set; }
    }
}
