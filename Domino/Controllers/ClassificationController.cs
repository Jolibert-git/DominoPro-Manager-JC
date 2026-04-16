using Domino.Application.Contract;
using Domino.Application.DTOs.ClassificationDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Domino.API.Controllers
{
    [ApiController]
    [Route("Api/Classification")]
    public class ClassificationController: ControllerBase
    {
        private readonly IClassificationService _service;

        public ClassificationController(IClassificationService service) => _service = service;

       
        
        [HttpGet("Tournament/{tournamentId:int}")]
        public async Task<IActionResult> GetByTournament(int tournamentId )
        {
            var response = await _service.GetByTournamentAsync(tournamentId )
                ;
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpGet("Tournament/{tournamentId:int}/Final")]
        public async Task<IActionResult> GetFinalStandings(int tournamentId )
        {
            var response = await _service.GetFinalStandingsAsync(tournamentId );

            return StatusCode(response.StatusCode, response);
        }

        
        [HttpGet("Tournament/{tournamentId:int}/Top/{n:int}")]
        public async Task<IActionResult> GetTopN(int tournamentId, int n )
        {
            var response = await _service.GetTopNAsync(tournamentId, n );
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpGet("Player/{playerId:int}/Tournament/{tournamentId:int}")]
        public async Task<IActionResult> GetByPlayerAndTournament(int playerId,int tournamentId)
        {
            var response = await _service.GetByPlayerAndTournamentAsync(playerId, tournamentId);

            return StatusCode(response.StatusCode, response);
        }

        

        [HttpPut]
        public async Task<IActionResult> Upsert( UpdateClassificationDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _service.UpsertAsync(request );
            return StatusCode(response.StatusCode, response);
        }

        
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _service.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
