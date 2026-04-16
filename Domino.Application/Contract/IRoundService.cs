using Domino.Application.DTOs.RoundDTOs;
using Domino.Application.Responses;

namespace Domino.Application.Contract
{
    public interface IRoundService
    {
        Task<ApiResponse<List<RoundDTO>>> GetByTournamentAsync(int tournamentId );
        Task<ApiResponse<RoundDTO>> GetByIdAsync(int id );
        Task<ApiResponse<RoundDetailDTO>> GetWithTablesAsync(int id );
        Task<ApiResponse<RoundDTO>> GetCurrentAsync(int tournamentId );
        Task<ApiResponse<RoundDTO>> CreateAsync(CreateRoundDTO request );
        Task<ApiResponse<RoundDTO>> UpdateStatusAsync(int id, UpdateRoundStatusDTO request );
        Task<ApiResponse<object>> DeleteAsync(int id );
    }
}
