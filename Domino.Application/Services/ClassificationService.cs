using AutoMapper;
using Domino.Application.Contract;
using Domino.Application.DTOs.ClassificationDTOs;
using Domino.Application.Responses;
using Domino.Domain.Entities;
using Domino.Infraestructure.Contracs;

namespace Domino.Application.Services
{
    public class ClassificationService: IClassificationService
    {
        private readonly IUnitOfWork _work;
        private readonly IMapper _mapper;

        public ClassificationService(IUnitOfWork work, IMapper mapper)
        {
            _work = work;
            _mapper = mapper;
        }


        public async Task<ApiResponse<List<ClassificationDTO>>> GetByTournamentAsync(int Id)
        {
            if (!await _work.Tournaments.ExistsAsync(Id))
            {
                return ApiResponse<List<ClassificationDTO>>.ErrorResponse($"Tournament with ID  was not found", 404);
            }

            var list = await _work.Classifications.GetByTournamentAsync(Id);

            return ApiResponse<List<ClassificationDTO>>.SuccessResponse(_mapper.Map<List<ClassificationDTO>>(list));
        }




        public async Task<ApiResponse<List<ClassificationDTO>>> GetFinalStandingsAsync(int Id)
        {
            if (!await _work.Tournaments.ExistsAsync(Id))
            {
                return ApiResponse<List<ClassificationDTO>>.ErrorResponse($"Tournament with ID  was not found", 404);
            }

            var list = await _work.Classifications.GetFinalStandingsAsync(Id);

            if (!list.Any())
            {
                return ApiResponse<List<ClassificationDTO>>.ErrorResponse("No final standings have been published for this tournament yet", 404);
            }

            return ApiResponse<List<ClassificationDTO>>.SuccessResponse(_mapper.Map<List<ClassificationDTO>>(list));
        }






        public async Task<ApiResponse<List<ClassificationDTO>>> GetTopNAsync(int tournamentId, int n)
        {
            if (n <= 0)
            {
                return ApiResponse<List<ClassificationDTO>>.ErrorResponse("N must be greater than 0", 400);
            }

            if (!await _work.Tournaments.ExistsAsync(tournamentId))
            {
                return ApiResponse<List<ClassificationDTO>>.ErrorResponse($"Tournament with ID was not found", 404);
            }

            var list = await _work.Classifications.GetTopNAsync(tournamentId, n);

            return ApiResponse<List<ClassificationDTO>>.SuccessResponse(_mapper.Map<List<ClassificationDTO>>(list));
        }



        public async Task<ApiResponse<ClassificationDTO>> GetByPlayerAndTournamentAsync(int playerId, int tournamentId)
        {
            var classification = await _work.Classifications.GetByPlayerAndTournamentAsync(playerId, tournamentId);

            if (classification is null)
            {
                return ApiResponse<ClassificationDTO>.ErrorResponse($"No classification found for player in tournament ", 404);
            }
            return ApiResponse<ClassificationDTO>.SuccessResponse(_mapper.Map<ClassificationDTO>(classification));
        }

        public async Task<ApiResponse<ClassificationDTO>> UpsertAsync(UpdateClassificationDTO updateClassificationDTO)
        {
            if (!await _work.Tournaments.ExistsAsync(updateClassificationDTO.TournamentId))
            {
                return ApiResponse<ClassificationDTO>.ErrorResponse($"Tournament with ID  was not found", 404);
            }

            if (!await _work.Players.ExistsAsync(updateClassificationDTO.PlayerId))
            {
                return ApiResponse<ClassificationDTO>.ErrorResponse($"Player with ID  was not found", 404);
            }

            var existing = await _work.Classifications.GetByPlayerAndTournamentAsync(updateClassificationDTO.PlayerId, updateClassificationDTO.TournamentId);

            if (existing is not null)
            {
                _mapper.Map(updateClassificationDTO, existing);

                _work.Classifications.Update(existing);
                await _work.CompleteAsync();

                return ApiResponse<ClassificationDTO>.SuccessResponse(_mapper.Map<ClassificationDTO>(existing));
            }
            var classification = _mapper.Map<Classification>(updateClassificationDTO);

            await _work.Classifications.AddAsync(classification);
            await _work.CompleteAsync();

            return ApiResponse<ClassificationDTO>.CreatedResponse(_mapper.Map<ClassificationDTO>(classification));
        }

        public async Task<ApiResponse<object>> DeleteAsync(int id)
        {
            var classification = await _work.Classifications.GetByIdAsync(id);

            if (classification is null)
            {
                return ApiResponse<object>.ErrorResponse($"Classification with ID  was not found", 404);
            }

            if (classification.IsFinal)
            {
                return ApiResponse<object>.ErrorResponse("Cannot delete a final classification record", 409);
            }
            _work.Classifications.Remove(classification);
            await _work.CompleteAsync();

            return ApiResponse<object>.SuccessResponse("Classification deleted successfully");
        }
    }
}
