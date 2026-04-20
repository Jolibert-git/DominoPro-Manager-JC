using AutoMapper;
using Domino.Application.Contract;
using Domino.Application.DTOs.TableDTOs;
using Domino.Application.Responses;
using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.Services
{
    public class TableService: ITableService
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public TableService(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }


        public async Task<ApiResponse<List<TableDTO>>> GetByRoundAsync(int roundId)
        {
            if (!await _work.Rounds.ExistsAsync(roundId))
            {
                return ApiResponse<List<TableDTO>>.ErrorResponse($"Round with ID  was not found", 404);
            }

            var tables = await _work.Tables.GetByRoundAsync(roundId);

            return ApiResponse<List<TableDTO>>.SuccessResponse(_mapper.Map<List<TableDTO>>(tables));
        }

        public async Task<ApiResponse<List<TableDTO>>> GetPendingByRoundAsync(int roundId)
        {
            if (!await _work.Rounds.ExistsAsync(roundId))
            {
                return ApiResponse<List<TableDTO>>.ErrorResponse($"Round with ID  was not found", 404);
            }

            var tables = await _work.Tables.GetPendingByRoundAsync(roundId);

            return ApiResponse<List<TableDTO>>.SuccessResponse(_mapper.Map<List<TableDTO>>(tables));
        }

        public async Task<ApiResponse<TableDTO>> GetByIdAsync(int id)
        {
            var table = await _work.Tables.GetByIdAsync(id);

            if (table is null)
            {
                return ApiResponse<TableDTO>.ErrorResponse($"Table with ID  was not found", 404);
            }

            return ApiResponse<TableDTO>.SuccessResponse(_mapper.Map<TableDTO>(table));
        }

        public async Task<ApiResponse<TableDetailDTO>> GetWithResultsAsync(int id)
        {
            var table = await _work.Tables.GetWithResultsAsync(id);

            if (table is null)
            {
                return ApiResponse<TableDetailDTO>.ErrorResponse($"Table with ID  was not found", 404);
            }

            return ApiResponse<TableDetailDTO>.SuccessResponse(_mapper.Map<TableDetailDTO>(table));
        }

        public async Task<ApiResponse<TableDTO>> CreateAsync(CreateTableDTO createTableDTO)
        {
            var round = await _work.Rounds.GetByIdAsync(createTableDTO.RoundId);

            if (round is null)
            {
                return ApiResponse<TableDTO>.ErrorResponse($"Round with ID  was not found", 404);
            }

            if (round.Status != RoundStatus.Pending && round.Status != RoundStatus.InPlay)
            {
                return ApiResponse<TableDTO>.ErrorResponse("Tables can only be created for rounds in 'Pending' or 'InPlay' status", 409);
            }

            var table = _mapper.Map<Table>(createTableDTO);

            await _work.Tables.AddAsync(table);
            await _work.CompleteAsync();

            return ApiResponse<TableDTO>.CreatedResponse(_mapper.Map<TableDTO>(table));
        }

        public async Task<ApiResponse<TableDTO>> UpdateStatusAsync(int id, UpdateTableStatusDTO updateTableDto)
        {
            var table = await _work.Tables.GetByIdAsync(id);

            if (table is null)
            {
                return ApiResponse<TableDTO>.ErrorResponse($"Table with ID  was not found", 404);
            }

            bool validTransition = (table.Status, updateTableDto.Status) switch
            {
                (GameStatus.Pending, GameStatus.InPlay) => true,
                (GameStatus.InPlay, GameStatus.Completed) => true,
                (GameStatus.Pending, GameStatus.Cancelled) => true,
                (GameStatus.InPlay, GameStatus.Cancelled) => true,
                _ => false
            };

            if (!validTransition)
            {
                return ApiResponse<TableDTO>.ErrorResponse($"Cannot transition from  to '{updateTableDto.Status}'", 409);
            }

            table.Status = updateTableDto.Status;

            if (updateTableDto.Status == GameStatus.InPlay && table.StartDate is null)
            {
                table.StartDate = DateTime.UtcNow;
            }

            if (updateTableDto.Status == GameStatus.Completed)
            {
                table.EndDate = updateTableDto.EndDate ?? DateTime.UtcNow;

                if (updateTableDto.WonBlock.HasValue)
                {
                    table.WonBlock = updateTableDto.WonBlock.Value;
                }

                if (updateTableDto.RemainingChips.HasValue)
                {
                    table.RemainingChips = updateTableDto.RemainingChips;
                }
            }

            _work.Tables.Update(table);
            await _work.CompleteAsync();

            return ApiResponse<TableDTO>.SuccessResponse(_mapper.Map<TableDTO>(table));
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            var table = await _work.Tables.GetByIdAsync(id);

            if (table is null)
            {
                return ApiResponse<object>.ErrorResponse($"Table with ID  was not found", 404);
            }

            if (table.Status != GameStatus.Pending)
            {
                return ApiResponse<object>.ErrorResponse("Only tables in 'Pending' status can be deleted", 409);
            }

            _work.Tables.Remove(table);
            await _work.CompleteAsync();

            return ApiResponse<object>.SuccessResponse("Table deleted successfully");
        }
    }
}
