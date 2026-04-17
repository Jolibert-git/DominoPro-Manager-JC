using Domino.Application.DTOs.ClassificationDTOs;
using Domino.Application.DTOs.RoundDTOs;
using Domino.Application.DTOs.TournamentRegistrationDTOs;

namespace Domino.Application.DTOs.TournamentDTOs
{
    public class TournamentDetailDTO: TournamentDTO
    {
        public IReadOnlyList<RoundSummaryDTO> Rounds { get; set; } = [];
        public IReadOnlyList<TournamentRegistrationDTO> Registrations { get; set; } = [];
        public IReadOnlyList<ClassificationDTO> Classifications { get; set; } = [];
    }
}
