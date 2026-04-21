using AutoMapper;
using Domino.Application.Contract;
using Domino.Application.DTOs.TournamentRegistrationDTOs;
using Domino.Application.Responses;
using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;
using static Domino.Domain.Entities.Enums;

namespace Domino.Application.Services
{
    public class TournamentRegistrationService: ITournamentRegistrationService
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public TournamentRegistrationService(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }

        public async Task<ApiResponse<List<TournamentRegistrationDTO>>> GetByTournamentAsync(int tournamentId)
        {
            if (!await _work.Tournaments.ExistsAsync(tournamentId))
            {
                return ApiResponse<List<TournamentRegistrationDTO>>.ErrorResponse($"Tournament with ID  was not found", 404);
            }

            var list = await _work.Registrations.GetByTournamentAsync(tournamentId);

            return ApiResponse<List<TournamentRegistrationDTO>>.SuccessResponse(_mapper.Map<List<TournamentRegistrationDTO>>(list));
        }

        public async Task<ApiResponse<List<TournamentRegistrationDTO>>> GetByPlayerAsync(int playerId)
        {
            if (!await _work.Players.ExistsAsync(playerId))
            {
                return ApiResponse<List<TournamentRegistrationDTO>>.ErrorResponse($"Player with ID  was not found", 404);
            }

            var list = await _work.Registrations.GetByPlayerAsync(playerId);

            return ApiResponse<List<TournamentRegistrationDTO>>.SuccessResponse(_mapper.Map<List<TournamentRegistrationDTO>>(list));
        }

        public async Task<ApiResponse<TournamentRegistrationDTO>> GetByPlayerAndTournamentAsync(int playerId, int tournamentId)
        {
            var reg = await _work.Registrations.GetByPlayerAndTournamentAsync(playerId, tournamentId);

            if (reg is null)
            {
                return ApiResponse<TournamentRegistrationDTO>.ErrorResponse($"No registration found for player  in tournament ", 404);
            }

            return ApiResponse<TournamentRegistrationDTO>.SuccessResponse(_mapper.Map<TournamentRegistrationDTO>(reg));
        }

        public async Task<ApiResponse<TournamentRegistrationDTO>> RegisterAsync(CreateTournamentRegistrationDTO request)
        {
            var tournament = await _work.Tournaments.GetByIdAsync(request.TournamentId);

            if (tournament is null)
            {
                return ApiResponse<TournamentRegistrationDTO>.ErrorResponse($"Tournament with ID  was not found", 404);
            }

            var player = await _work.Players.GetByIdAsync(request.PlayerId);

            if (player is null)
            {
                return ApiResponse<TournamentRegistrationDTO>.ErrorResponse($"Player with ID  was not found", 404);
            }

            if (tournament.Status != TournamentStatus.Programmed)
            {
                return ApiResponse<TournamentRegistrationDTO>.ErrorResponse("Registrations are only allowed for tournaments in 'Programmed' status", 409);
            }

            if (tournament.IsFull)
            {
                return ApiResponse<TournamentRegistrationDTO>.ErrorResponse("This tournament has reached its maximum number of players", 409);
            }

            if (!player.IsActive)
            {
                return ApiResponse<TournamentRegistrationDTO>.ErrorResponse("Inactive players cannot be registered in a tournament", 409);
            }

            if (await _work.Registrations.IsPlayerRegisteredAsync(request.PlayerId, request.TournamentId))
            {
                return ApiResponse<TournamentRegistrationDTO>.ErrorResponse("This player is already registered in the tournament", 409);
            }

            await _work.BeginTransactionAsync();

            try
            {
                var registration = _mapper.Map<TournamentRegistration>(request);

                await _work.Registrations.AddAsync(registration);
                await _work.CompleteAsync();
                await _work.CommitAsync();

                return ApiResponse<TournamentRegistrationDTO>.CreatedResponse(_mapper.Map<TournamentRegistrationDTO>(registration));
            }
            catch
            {
                await _work.RollbackAsync();
                throw;
            }
        }

        public async Task<ApiResponse<TournamentRegistrationDTO>> UpdateStatusAsync(int id, UpdateTournamentRegistrationStatusDTO UpdateTournamentStatusDTO)
        {
            var reg = await _work.Registrations.GetByIdAsync(id);

            if (reg is null)
            {
                return ApiResponse<TournamentRegistrationDTO>.ErrorResponse($"Registration with ID  was not found", 404);
            }

            reg.Status = UpdateTournamentStatusDTO.Status;

            if (UpdateTournamentStatusDTO.PaymentFee.HasValue)
            {
                reg.PaymentFee = UpdateTournamentStatusDTO.PaymentFee.Value;
            }

            _work.Registrations.Update(reg);
            await _work.CompleteAsync();

            return ApiResponse<TournamentRegistrationDTO>.SuccessResponse(_mapper.Map<TournamentRegistrationDTO>(reg));
        }

        public async Task<ApiResponse<object>> WithdrawAsync(int id)
        {
            var reg = await _work.Registrations.GetByIdAsync(id);

            if (reg is null)
            {
                return ApiResponse<object>.ErrorResponse($"Registration with ID  was not found", 404);
            }

            if (reg.Status == RegistrationStatus.Withdrawn)
            {
                return ApiResponse<object>.ErrorResponse("This registration is already withdrawn", 409);
            }

            if (reg.Status == RegistrationStatus.Disqualified)
            {
                return ApiResponse<object>.ErrorResponse("Cannot withdraw a disqualified registration", 409);
            }

            reg.Status = RegistrationStatus.Withdrawn;

            _work.Registrations.Update(reg);
            await _work.CompleteAsync();

            return ApiResponse<object>.SuccessResponse("Player withdrawn from tournament successfully");
        }
    }
}
