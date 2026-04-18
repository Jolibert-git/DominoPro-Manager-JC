using AutoMapper;
using Domino.Application.Contract;
using Domino.Application.DTOs.PlayerDTOs;
using Domino.Application.Responses;
using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;


namespace Domino.Application.Services
{
    public class PlayerService: IPlayerService
    {
        private readonly IUnitOfWork _work ;
        private readonly IMapper _mapper;

        public PlayerService(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }


        public async Task<ApiResponse<List<PlayerDTO>>> GetAllAsync()
        {
            var players = await _work.Players.GetAllAsync();

            return ApiResponse<List<PlayerDTO>>.SuccessResponse(_mapper.Map<List<PlayerDTO>>(players));
        }

        public async Task<ApiResponse<List<PlayerDTO>>> GetActiveAsync()
        {
            var players = await _work.Players.GetActivePlayersAsync();
            return ApiResponse<List<PlayerDTO>>.SuccessResponse(_mapper.Map<List<PlayerDTO>>(players));
        }

        public async Task<ApiResponse<List<PlayerDTO>>> GetByEloRangeAsync(short min, short max )
        {
            if (min > max)
            {
                return ApiResponse<List<PlayerDTO>>.ErrorResponse("Min Elo cannot be greater than Max Elo", 400);
            }
            var players = await _work.Players.GetByEloRangeAsync(min, max);

            return ApiResponse<List<PlayerDTO>>.SuccessResponse(_mapper.Map<List<PlayerDTO>>(players));
        }

        public async Task<ApiResponse<PlayerDTO>> GetByIdAsync(int id)
        {
            var player = await _work.Players.GetByIdAsync(id);

            if (player is null)
            {
                return ApiResponse<PlayerDTO>.ErrorResponse($"Player with ID  was not found", 404);
            }

            return ApiResponse<PlayerDTO>.SuccessResponse(_mapper.Map<PlayerDTO>(player));
        }

        public async Task<ApiResponse<PlayerDTO>> GetByEmailAsync(string email)
        {
            var player = await _work.Players.GetByEmailAsync(email);

            if (player is null)
            {
                return ApiResponse<PlayerDTO>.ErrorResponse($"Player with email  was not found", 404);
            }
            return ApiResponse<PlayerDTO>.SuccessResponse(_mapper.Map<PlayerDTO>(player));
        }

        public async Task<ApiResponse<PlayerDTO>> GetWithStatsAsync(int id )
        {
            var player = await _work.Players.GetWithStatsAsync(id );

            if (player is null)
            {
                return ApiResponse<PlayerDTO>.ErrorResponse($"Player with ID was not found", 404);
            }
            return ApiResponse<PlayerDTO>.SuccessResponse(_mapper.Map<PlayerDTO>(player));
        }

        public async Task<ApiResponse<PlayerDTO>> CreateAsync(CreatePlayerDTO request)
        {
            var existing = await _work.Players.GetByEmailAsync(request.Email);

            if (existing is not null)
            {
                return ApiResponse<PlayerDTO>.ErrorResponse($"A player with email already exists", 409);
            }

            var player = _mapper.Map<Player>(request);
            player.Name = player.Name.Trim();
            player.LastName = player.LastName.Trim();
            player.Email = player.Email.Trim().ToLower();
            player.Phone = player.Phone.Trim();

            await _work.Players.AddAsync(player);
            await _work.CompleteAsync();

            return ApiResponse<PlayerDTO>.CreatedResponse(_mapper.Map<PlayerDTO>(player));
        }

        public async Task<ApiResponse<PlayerDTO>> UpdateAsync(int id, UpdatePlayerDTO request)
        {
            var player = await _work.Players.GetByIdAsync(id);

            if (player is null)
            {
                return ApiResponse<PlayerDTO>.ErrorResponse($"Player with ID  was not found", 404);
            }

            if (request.Email is not null && !request.Email.Equals(player.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailTaken = await _work.Players.GetByEmailAsync(request.Email);

                if (emailTaken is not null)
                {
                    return ApiResponse<PlayerDTO>.ErrorResponse($"Email  is already in use by another player", 409);
                }
            }

            _mapper.Map(request, player);

            _work.Players.Update(player);
            await _work.CompleteAsync();

            return ApiResponse<PlayerDTO>.SuccessResponse(_mapper.Map<PlayerDTO>(player));
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            var player = await _work.Players.GetByIdAsync(id);

            if (player is null)
            {
                return ApiResponse<object>.ErrorResponse($"Player with ID was not found", 404);
            }

            player.IsActive = false;

            _work.Players.Update(player);
            await _work.CompleteAsync();

            return ApiResponse<object>.SuccessResponse("Player deactivated successfully");
        }
    }
}
