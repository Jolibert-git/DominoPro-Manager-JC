using Domino.Application.DTOs.ResultDTOs;
using Domino.Application.Responses;

namespace Domino.Application.Contract
{
    public interface IResultService
    {
        Task<ApiResponse<List<ResultDTO>>> GetByTableAsync(int tableId );
        Task<ApiResponse<List<ResultDTO>>> GetByPlayerAsync(int playerId );
        Task<ApiResponse<List<ResultDTO>>> GetByPlayerAndTournamentAsync(int playerId, int tournamentId );
        Task<ApiResponse<ResultDTO>> GetByIdAsync(int id );
        Task<ApiResponse<ResultDTO>> GetWinnerOfTableAsync(int tableId );
        Task<ApiResponse<ResultDTO>> CreateAsync(CreateResultDTO request );
        Task<ApiResponse<ResultDTO>> UpdateAsync(int id, UpdateResultDTO requestm );
        Task<ApiResponse<object>> DeleteAsync(int id );
    }
}
