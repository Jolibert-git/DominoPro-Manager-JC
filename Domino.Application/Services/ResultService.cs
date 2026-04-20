using AutoMapper;
using Azure.Core;
using Domino.Application.Contract;
using Domino.Application.DTOs.ResultDTOs;
using Domino.Application.Responses;
using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.Services
{
    public class ResultService: IResultService
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public ResultService(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }


        public async Task<ApiResponse<List<ResultDTO>>> GetByTableAsync(int Id )
        {
            if (!await _work.Tables.ExistsAsync(Id))
            {
                return ApiResponse<List<ResultDTO>>.ErrorResponse($"Table with ID  not found", 404);
            }

            var results = await _work.Results.GetByTableAsync(Id );
            return ApiResponse<List<ResultDTO>>.SuccessResponse(_mapper.Map<List<ResultDTO>>(results));
        }

        public async Task<ApiResponse<List<ResultDTO>>> GetByPlayerAsync(int Id )
        {
            if (!await _work.Players.ExistsAsync(Id))
            {
                return ApiResponse<List<ResultDTO>>.ErrorResponse($"Player with ID  not found", 404);
            }

            var results = await _work.Results.GetByPlayerAsync(Id );
            return ApiResponse<List<ResultDTO>>.SuccessResponse(_mapper.Map<List<ResultDTO>>(results));
        }

        public async Task<ApiResponse<List<ResultDTO>>> GetByPlayerAndTournamentAsync(int playerId, int tournamentId )
        {
            if (!await _work.Players.ExistsAsync(playerId))
            {
                return ApiResponse<List<ResultDTO>>.ErrorResponse($"Player with ID  was not found", 404);
            }
               
            if (!await _work.Tournaments.ExistsAsync(tournamentId))
            {
                return ApiResponse<List<ResultDTO>>.ErrorResponse($"Tournament with ID  was not found", 404);
            }   

            var results = await _work.Results.GetByPlayerAndTournamentAsync(playerId, tournamentId);
            return ApiResponse<List<ResultDTO>>.SuccessResponse(_mapper.Map<List<ResultDTO>>(results));
        }

        public async Task<ApiResponse<ResultDTO>> GetByIdAsync(int id )
        {
            var result = await _work.Results.GetByIdAsync(id );
            if (result is null)
            {
                return ApiResponse<ResultDTO>.ErrorResponse($"Result with ID  was not found", 404);
            }

            return ApiResponse<ResultDTO>.SuccessResponse(_mapper.Map<ResultDTO>(result));
        }

        public async Task<ApiResponse<ResultDTO>> GetWinnerOfTableAsync(int Id )
        {
            if (!await _work.Tables.ExistsAsync(Id))
            {
                return ApiResponse<ResultDTO>.ErrorResponse($"Table with ID  was not found", 404);
            }

            var result = await _work.Results.GetWinnerOfTableAsync(Id );

            if (result is null)
            {
                return ApiResponse<ResultDTO>.ErrorResponse($"No winner registered yet for table ", 404);
            }

            return ApiResponse<ResultDTO>.SuccessResponse(_mapper.Map<ResultDTO>(result));
        }

        public async Task<ApiResponse<ResultDTO>> CreateAsync(CreateResultDTO resultDto)
        {
            var table = await _work.Tables.GetByIdAsync(resultDto.TableId);

            if (table is null)
            {
                return ApiResponse<ResultDTO>.ErrorResponse($"Table with ID {resultDto.TableId} was not found", 404);
            }

            if (table.Status == GameStatus.Completed || table.Status == GameStatus.Cancelled)
            {
                return ApiResponse<ResultDTO>.ErrorResponse("Cannot add results to a completed or cancelled table", 409);
            }

            if (!await _work.Players.ExistsAsync(resultDto.PlayerId))
            {
                return ApiResponse<ResultDTO>.ErrorResponse($"Player with ID  was not found", 404);
            }

            if (resultDto.PlaymateId.HasValue)
            {
                if (resultDto.PlaymateId == resultDto.PlayerId)
                {
                    return ApiResponse<ResultDTO>.ErrorResponse("A player cannot be their own playmate", 400);
                }

                if (!await _work.Players.ExistsAsync(resultDto.PlaymateId.Value))
                {
                    return ApiResponse<ResultDTO>.ErrorResponse($"Playmate with ID  was not found", 404);
                }
            }

            if (resultDto.IsWinner)
            {
                var winner = await _work.Results.GetWinnerOfTableAsync(resultDto.TableId);

                if (winner is not null)
                {
                    return ApiResponse<ResultDTO>.ErrorResponse("This table already has a winner registered", 409);
                }
            }

            await _work.BeginTransactionAsync();

            try
            {
                var result = _mapper.Map<Result>(resultDto);
                await _work.Results.AddAsync(result);

                var player = await _work.Players.GetByIdAsync(resultDto.PlayerId);


                player!.Elo = (short)Math.Clamp(player.Elo + resultDto.Elo, 0, 3000);

                player.WinGame = resultDto.IsWinner ? player.WinGame + 1 : player.WinGame;
                player.LostGame = !resultDto.IsWinner ? player.LostGame + 1 : player.LostGame;

                _work.Players.Update(player);

                await _work.CommitAsync();

                return ApiResponse<ResultDTO>.CreatedResponse(_mapper.Map<ResultDTO>(result));
            }
            catch
            {
                await _work.RollbackAsync();
                throw;
            }
        }


        public async Task<ApiResponse<ResultDTO>> UpdateAsync(int id, UpdateResultDTO updateResultDTO)
        {
            var result = await _work.Results.GetByIdAsync(id);

            if (result is null)
            {
                return ApiResponse<ResultDTO>.ErrorResponse($"Result with ID  was not found", 404);
            }

            _mapper.Map(updateResultDTO, result);

            _work.Results.Update(result);
            await _work.CompleteAsync();

            return ApiResponse<ResultDTO>.SuccessResponse(_mapper.Map<ResultDTO>(result));
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            var result = await _work.Results.GetByIdAsync(id);

            if (result is null)
            {
                return ApiResponse<object>.ErrorResponse($"Result with ID  was not found", 404);
            }

            _work.Results.Remove(result);
            await _work.CompleteAsync();

            return ApiResponse<object>.SuccessResponse("Deleted successfully");
        }
    }
}
