using Domino.Application.DTOs.PlayerDTOs;
using Domino.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domino.Application.Contract
{
    public interface IPlayerService
    {
        Task<ApiResponse<List<PlayerDTO>>> GetAllAsync();
        Task<ApiResponse<List<PlayerDTO>>> GetActiveAsync();
        Task<ApiResponse<List<PlayerDTO>>> GetByEloRangeAsync(short min, short max );
        Task<ApiResponse<PlayerDTO>> GetByIdAsync(int id );
        Task<ApiResponse<PlayerDTO>> GetByEmailAsync(string email );
        Task<ApiResponse<PlayerDTO>> GetWithStatsAsync(int id );
        Task<ApiResponse<PlayerDTO>> CreateAsync(CreatePlayerDTO request);
        Task<ApiResponse<PlayerDTO>> UpdateAsync(int id, UpdatePlayerDTO request );
        Task<ApiResponse<object>> DeleteAsync(int id );
    }
}
