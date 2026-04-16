using Domino.Application.DTOs.TournamentRegistrationDTOs;
using Domino.Application.Responses;


namespace Domino.Application.Contract
{
    public interface ITournamentRegistrationService
    {
        Task<ApiResponse<List<TournamentRegistrationDTO>>> GetByTournamentAsync(int tournamentId );
        Task<ApiResponse<List<TournamentRegistrationDTO>>> GetByPlayerAsync(int playerId );
        Task<ApiResponse<TournamentRegistrationDTO>> GetByPlayerAndTournamentAsync(int playerId, int tournamentId );
        Task<ApiResponse<TournamentRegistrationDTO>> RegisterAsync(CreateTournamentRegistrationDTO request );
        Task<ApiResponse<TournamentRegistrationDTO>> UpdateStatusAsync(int id, UpdateTournamentRegistrationStatusDTO request );
        Task<ApiResponse<object>> WithdrawAsync(int id );
    }
}
