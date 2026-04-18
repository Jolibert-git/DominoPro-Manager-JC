using Domino.Application.Contract;
using Domino.Application.DTOs.RoundDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Domino.API.Controllers
{
    [ApiController]
    [Route("Api/Rounds")]
    public class RoundController: ControllerBase
    {
        private readonly IRoundService _service;

        public RoundController(IRoundService service) => _service = service;

    
        [HttpGet("Tournament/{tournamentId:int}")]
        public async Task<IActionResult> GetByTournament(int tournamentId  )
        {
            var response = await _service.GetByTournamentAsync(tournamentId);
            return StatusCode(response.StatusCode, response);
        }

    
        [HttpGet("Tournament/{tournamentId:int}/Current")]
        public async Task<IActionResult> GetCurrent(int tournamentId)
        {
            var response = await _service.GetCurrentAsync(tournamentId);
            return StatusCode(response.StatusCode, response);
        }



        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _service.GetByIdAsync(id);

            return StatusCode(response.StatusCode, response);
        }


        [HttpGet("{id:int}/Tables")]
        public async Task<IActionResult> GetWithTables(int id)
        {
            var response = await _service.GetWithTablesAsync(id);
            return StatusCode(response.StatusCode, response);
        }


        [HttpPost]
        public async Task<IActionResult> Create( CreateRoundDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _service.CreateAsync(request);

            return StatusCode(response.StatusCode, response);
        }



        [HttpPatch("{id:int}/Status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateRoundStatusDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _service.UpdateStatusAsync(id, request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id )
        {
            var response = await _service.DeleteAsync(id );
            return StatusCode(response.StatusCode, response);
        }
    }
}
