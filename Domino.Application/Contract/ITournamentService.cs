using Domino.Application.DTOs.TournamentDTOs;
using Domino.Application.Responses;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.Contract
{
    public interface ITournamentService
    {
        Task<ApiResponse<List<TournamentDTO>>> GetAllByStatusAsync(TournamentStatus status );
        Task<ApiResponse<List<TournamentDTO>>> GetActiveAsync( );
        Task<ApiResponse<TournamentDTO>> GetByIdAsync(int id );
        Task<ApiResponse<TournamentDetailDTO>> GetFullAsync(int id );
        Task<ApiResponse<TournamentDTO>> CreateAsync(CreateTournamentDTO request );
        Task<ApiResponse<TournamentDTO>> UpdateAsync(int id, UpdateTournamentDTO request);
        Task<ApiResponse<TournamentDTO>> UpdateStatusAsync(int id, UpdateTournamentStatusDTO request );
        Task<ApiResponse<object>> DeleteAsync(int id );
    }
}
