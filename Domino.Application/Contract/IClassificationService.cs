using Domino.Application.DTOs.ClassificationDTOs;
using Domino.Application.Responses;


namespace Domino.Application.Contract
{
    public interface IClassificationService
    {
        Task<ApiResponse<List<ClassificationDTO>>> GetByTournamentAsync(int tournamentId );
        Task<ApiResponse<List<ClassificationDTO>>> GetFinalStandingsAsync(int tournamentId );
        Task<ApiResponse<List<ClassificationDTO>>> GetTopNAsync(int tournamentId, int n );
        Task<ApiResponse<ClassificationDTO>> GetByPlayerAndTournamentAsync(int playerId, int tournamentId );
        Task<ApiResponse<ClassificationDTO>> UpsertAsync(UpdateClassificationDTO request );
        Task<ApiResponse<object>> DeleteAsync(int id );
    }
}
