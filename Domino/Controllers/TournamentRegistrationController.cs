using Domino.Application.Contract;
using Domino.Application.DTOs.TournamentRegistrationDTOs;
using Domino.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Domino.API.Controllers
{
    [ApiController]
    [Route("Api/TournamentRegistration")]
    public class TournamentRegistrationController: ControllerBase
    {
        private readonly ITournamentRegistrationService _service;

        public TournamentRegistrationController(ITournamentRegistrationService service)
            => _service = service;


        [HttpGet("Tournament/{tournamentId:int}")]
        public async Task<IActionResult> GetByTournament(int tournamentId)
        {
            var response = await _service.GetByTournamentAsync(tournamentId);

            return StatusCode(response.StatusCode, response);
        }


        [HttpGet("Player/{playerId:int}")]
        public async Task<IActionResult> GetByPlayer(int playerId)
        {
            var response = await _service.GetByPlayerAsync(playerId);
            return StatusCode(response.StatusCode, response);
        }



        [HttpGet("Player/{playerId:int}/Tournament/{tournamentId:int}")]
        public async Task<IActionResult> GetByPlayerAndTournament(int playerId, int tournamentId)
        {
            var response = await _service.GetByPlayerAndTournamentAsync(playerId, tournamentId);
            return StatusCode(response.StatusCode, response);
        }



        [HttpPost]
        public async Task<IActionResult> Register( CreateTournamentRegistrationDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _service.RegisterAsync(request);
            return StatusCode(response.StatusCode, response);
        }



        [HttpPatch("{id:int}/Status")]
        public async Task<IActionResult> UpdateStatus(int id,  UpdateTournamentRegistrationStatusDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response = await _service.UpdateStatusAsync(id, request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("{id:int}/Withdraw")]
        public async Task<IActionResult> Withdraw(int id)
        {
            var response = await _service.WithdrawAsync(id);

            return StatusCode(response.StatusCode, response);
        }
    }
}
