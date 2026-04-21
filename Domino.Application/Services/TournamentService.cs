using AutoMapper;
using Domino.Application.Contract;
using Domino.Application.DTOs.TournamentDTOs;
using Domino.Application.Responses;
using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.Services
{
    public class TournamentService: ITournamentService
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public TournamentService(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }


        public async Task<ApiResponse<List<TournamentDTO>>> GetAllByStatusAsync(TournamentStatus status)
        {
            var list = await _work.Tournaments.GetByStatusAsync(status);

            return ApiResponse<List<TournamentDTO>>.SuccessResponse(_mapper.Map<List<TournamentDTO>>(list));
        }

        public async Task<ApiResponse<List<TournamentDTO>>> GetActiveAsync()
        {
            var list = await _work.Tournaments.GetActiveAsync();

            return ApiResponse<List<TournamentDTO>>.SuccessResponse(_mapper.Map<List<TournamentDTO>>(list));
        }

        public async Task<ApiResponse<TournamentDTO>> GetByIdAsync(int id)
        {
            var tournament = await _work.Tournaments.GetByIdAsync(id);

            if (tournament is null)
            {
                return ApiResponse<TournamentDTO>.ErrorResponse($"Tournament with ID  was not found", 404);
            }

            return ApiResponse<TournamentDTO>.SuccessResponse(_mapper.Map<TournamentDTO>(tournament));
        }

        public async Task<ApiResponse<TournamentDetailDTO>> GetFullAsync(int id)
        {
            var tournament = await _work.Tournaments.GetFullAsync(id);

            if (tournament is null)
            {
                return ApiResponse<TournamentDetailDTO>.ErrorResponse($"Tournament with ID  was not found", 404);
            }

            return ApiResponse<TournamentDetailDTO>.SuccessResponse(_mapper.Map<TournamentDetailDTO>(tournament));
        }

        public async Task<ApiResponse<TournamentDTO>> CreateAsync(CreateTournamentDTO createtournamentDTO)
        {
            if (createtournamentDTO.EndDate.HasValue && createtournamentDTO.EndDate <= createtournamentDTO.StartDate)
            {
                return ApiResponse<TournamentDTO>.ErrorResponse("End date must be after start date", 400);
            }

            if (createtournamentDTO.MaxPlayers.HasValue && createtournamentDTO.MaxPlayers < createtournamentDTO.MinPlayers)
            {
                return ApiResponse<TournamentDTO>.ErrorResponse("Max players cannot be less than min players", 400);
            }

            var tournament = _mapper.Map<Tournament>(createtournamentDTO);

            await _work.Tournaments.AddAsync(tournament);
            await _work.CompleteAsync();

            return ApiResponse<TournamentDTO>.CreatedResponse(_mapper.Map<TournamentDTO>(tournament));
        }

        public async Task<ApiResponse<TournamentDTO>> UpdateAsync(int id, UpdateTournamentDTO updateTournamentDTO)
        {
            var tournament = await _work.Tournaments.GetByIdAsync(id);

            if (tournament is null)
            {
                return ApiResponse<TournamentDTO>.ErrorResponse($"Tournament with ID  was not found", 404);
            }

            if (tournament.Status == TournamentStatus.Finalized || tournament.Status == TournamentStatus.Canceled)
            {
                return ApiResponse<TournamentDTO>.ErrorResponse("Cannot update a finalized or canceled tournament", 409);
            }

            if (updateTournamentDTO.EndDate.HasValue && updateTournamentDTO.EndDate <= (updateTournamentDTO.StartDate ?? tournament.StartDate))
            {
                return ApiResponse<TournamentDTO>.ErrorResponse("End date must be after start date", 400);
            }

            _mapper.Map(updateTournamentDTO, tournament);

            _work.Tournaments.Update(tournament);
            await _work.CompleteAsync();

            return ApiResponse<TournamentDTO>.SuccessResponse(_mapper.Map<TournamentDTO>(tournament));
        }


        public async Task<ApiResponse<TournamentDTO>> UpdateStatusAsync(int id, UpdateTournamentStatusDTO updateTournamentStatusDTO)
        {
            var tournament = await _work.Tournaments.GetByIdAsync(id);

            if (tournament is null)
            {
                return ApiResponse<TournamentDTO>.ErrorResponse($"Tournament with ID  was not found", 404);
            }

            bool validTransition = (tournament.Status, updateTournamentStatusDTO.Status) switch
            {
                (TournamentStatus.Programmed, TournamentStatus.InCourse) => true,
                (TournamentStatus.InCourse, TournamentStatus.Finalized) => true,
                (TournamentStatus.Programmed, TournamentStatus.Canceled) => true,
                (TournamentStatus.InCourse, TournamentStatus.Canceled) => true,
                _ => false
            };

            if (!validTransition)
            {
                return ApiResponse<TournamentDTO>.ErrorResponse($"Cannot transition ", 409);
            }

            tournament.Status = updateTournamentStatusDTO.Status;

            if (updateTournamentStatusDTO.Status == TournamentStatus.Finalized)
            {
                tournament.EndDate = DateTime.UtcNow;
            }

            _work.Tournaments.Update(tournament);
            await _work.CompleteAsync();

            return ApiResponse<TournamentDTO>.SuccessResponse(_mapper.Map<TournamentDTO>(tournament));
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            var tournament = await _work.Tournaments.GetByIdAsync(id);

            if (tournament is null)
            {
                return ApiResponse<object>.ErrorResponse($"Tournament with ID  was not found", 404);
            }

            if (tournament.Status == TournamentStatus.InCourse)
            {
                return ApiResponse<object>.ErrorResponse("Cannot delete a tournament that is currently in progress", 409);
            }

            _work.Tournaments.Remove(tournament);
            await _work.CompleteAsync();

            return ApiResponse<object>.SuccessResponse("Tournament deleted successfully");
        }
    }
}
