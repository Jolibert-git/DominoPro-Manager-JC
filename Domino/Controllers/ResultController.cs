using Domino.Application.Contract;
using Domino.Application.DTOs.ResultDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Domino.API.Controllers
{
    [ApiController]
    [Route("Api/Results")]
    public class ResultController: ControllerBase
    {
        private readonly IResultService _service;

        public ResultController(IResultService service) => _service = service;

  
        [HttpGet("Table/{tableId:int}")]
        public async Task<IActionResult> GetByTable(int tableId )
        {
            var response = await _service.GetByTableAsync(tableId);
            return StatusCode(response.StatusCode, response);
        }


        [HttpGet("Table/{tableId:int}/Winner")]
        public async Task<IActionResult> GetWinnerOfTable(int tableId )
        {
            var response = await _service.GetWinnerOfTableAsync(tableId );

            return StatusCode(response.StatusCode, response);
        }


        [HttpGet("Player/{playerId:int}")]
        public async Task<IActionResult> GetByPlayer(int playerId )
        {
            var response = await _service.GetByPlayerAsync(playerId );

            return StatusCode(response.StatusCode, response);
        }


        [HttpGet("Player/{playerId:int}/Tournament/{tournamentId:int}")]
        public async Task<IActionResult> GetByPlayerAndTournament(int playerId,int tournamentId)
        {
            var response = await _service.GetByPlayerAndTournamentAsync(playerId, tournamentId);

            return StatusCode(response.StatusCode, response);
        }

       
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id )
        {
            var response = await _service.GetByIdAsync(id );
            return StatusCode(response.StatusCode, response);
        }



        [HttpPost]
        public async Task<IActionResult> Create( CreateResultDTO request )
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _service.CreateAsync(request );
            return StatusCode(response.StatusCode, response);
        }

        

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update( int id, UpdateResultDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _service.UpdateAsync(id, request );
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
