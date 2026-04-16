using Domino.Application.DTOs.TableDTOs;
using Domino.Application.Responses;


namespace Domino.Application.Contract
{
    public interface ITableService
    {
        Task<ApiResponse<List<TableDTO>>> GetByRoundAsync(int roundId );
        Task<ApiResponse<List<TableDTO>>> GetPendingByRoundAsync(int roundId );
        Task<ApiResponse<TableDTO>> GetByIdAsync(int id );
        Task<ApiResponse<TableDetailDTO>> GetWithResultsAsync(int id );
        Task<ApiResponse<TableDTO>> CreateAsync(CreateTableDTO request );
        Task<ApiResponse<TableDTO>> UpdateStatusAsync(int id, UpdateTableStatusDTO request );
        Task<ApiResponse<object>> DeleteAsync(int id );
    }
}
