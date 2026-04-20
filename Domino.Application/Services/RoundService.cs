using AutoMapper;
using Domino.Application.Contract;
using Domino.Application.DTOs.RoundDTOs;
using Domino.Application.Responses;
using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.Services
{
    public class RoundService: IRoundService
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public RoundService(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }


        public async Task<ApiResponse<List<RoundDTO>>> GetByTournamentAsync(int Id)
        {
            if (!await _work.Tournaments.ExistsAsync(Id))
            {
                return ApiResponse<List<RoundDTO>>.ErrorResponse($"Tournament with ID was not found", 404);
            }

            var rounds = await _work.Rounds.GetByTournamentAsync(Id);

            return ApiResponse<List<RoundDTO>>.SuccessResponse(_mapper.Map<List<RoundDTO>>(rounds));
        }

        public async Task<ApiResponse<RoundDTO>> GetByIdAsync(int id)
        {
            var round = await _work.Rounds.GetByIdAsync(id);

            if (round is null)
            {
                return ApiResponse<RoundDTO>.ErrorResponse($"Round with ID was not found", 404);
            }

            return ApiResponse<RoundDTO>.SuccessResponse(_mapper.Map<RoundDTO>(round));
        }

        public async Task<ApiResponse<RoundDetailDTO>> GetWithTablesAsync(int id)
        {
            var round = await _work.Rounds.GetWithTablesAsync(id);

            if (round is null)
            {
                return ApiResponse<RoundDetailDTO>.ErrorResponse($"Round with ID  was not found", 404);
            }

            return ApiResponse<RoundDetailDTO>.SuccessResponse(_mapper.Map<RoundDetailDTO>(round));
        }

        public async Task<ApiResponse<RoundDTO>> GetCurrentAsync(int tournamentId)
        {
            var round = await _work.Rounds.GetCurrentAsync(tournamentId);

            if (round is null)
            {
                return ApiResponse<RoundDTO>.ErrorResponse($"No active round found for tournament", 404);
            }

            return ApiResponse<RoundDTO>.SuccessResponse(_mapper.Map<RoundDTO>(round));
        }

        public async Task<ApiResponse<RoundDTO>> CreateAsync(CreateRoundDTO createRoundDto)
        {
            var tournament = await _work.Tournaments.GetByIdAsync(createRoundDto.TournamentId);

            if (tournament is null)
            {
                return ApiResponse<RoundDTO>.ErrorResponse($"Tournament with ID was not found", 404);
            }

            if (tournament.Status != TournamentStatus.InCourse)
            {
                return ApiResponse<RoundDTO>.ErrorResponse("Rounds can only be created for tournaments that are 'InCourse'", 409);
            }

            var activeRound = await _work.Rounds.GetCurrentAsync(createRoundDto.TournamentId);

            if (activeRound is not null)
            {
                return ApiResponse<RoundDTO>.ErrorResponse($"Round {activeRound.RoundNumber} is still in progress. Complete it before creating a new one", 409);
            }

            if (tournament.MaxRound.HasValue && createRoundDto.RoundNumber > tournament.MaxRound.Value)
            {
                return ApiResponse<RoundDTO>.ErrorResponse($"Round number exceeds the maximum allowed ", 400);
            }

            var round = _mapper.Map<Round>(createRoundDto);

            await _work.Rounds.AddAsync(round);
            await _work.CompleteAsync();

            return ApiResponse<RoundDTO>.CreatedResponse(_mapper.Map<RoundDTO>(round));
        }

        public async Task<ApiResponse<RoundDTO>> UpdateStatusAsync(int id, UpdateRoundStatusDTO roundStatusDto)
        {
            var round = await _work.Rounds.GetByIdAsync(id);

            if (round is null)
            {
                return ApiResponse<RoundDTO>.ErrorResponse($"Round with ID was not found", 404);
            }

            bool validTransition = (round.Status, roundStatusDto.Status) switch
            {
                (RoundStatus.Pending, RoundStatus.InPlay) => true,
                (RoundStatus.InPlay, RoundStatus.Completed) => true,
                _ => false
            };

            if (!validTransition)
            {
                return ApiResponse<RoundDTO>.ErrorResponse($"Cannot transition", 409);
            }

            if (roundStatusDto.Status == RoundStatus.Completed)
            {
                var roundWithTables = await _work.Rounds.GetWithTablesAsync(id);

                if (roundWithTables!.Tables.Any(t => !t.IsComplete))
                {
                    return ApiResponse<RoundDTO>.ErrorResponse("Cannot complete a round that still has pending tables", 409);
                }
            }

            round.Status = roundStatusDto.Status;

            if (roundStatusDto.Status == RoundStatus.InPlay && round.StartDate is null)
            {
                round.StartDate = DateTime.UtcNow;
            }

            if (roundStatusDto.Status == RoundStatus.Completed)
            {
                round.EndDate = roundStatusDto.EndDate ?? DateTime.UtcNow;
            }

            _work.Rounds.Update(round);
            await _work.CompleteAsync();

            return ApiResponse<RoundDTO>.SuccessResponse(_mapper.Map<RoundDTO>(round));
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            var round = await _work.Rounds.GetByIdAsync(id);

            if (round is null)
            {
                return ApiResponse<object>.ErrorResponse($"Round with ID  was not found", 404);
            }

            if (round.Status != RoundStatus.Pending)
            {
                return ApiResponse<object>.ErrorResponse("Only rounds in 'Pending' status can be deleted", 409);
            }

            _work.Rounds.Remove(round);
            await _work.CompleteAsync();

            return ApiResponse<object>.SuccessResponse("Round deleted successfully");
        }
    }
}
